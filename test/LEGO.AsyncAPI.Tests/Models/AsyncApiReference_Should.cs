// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.Interface;
    using NUnit.Framework;

    public class AsyncApiReference_Should : TestBase
    {
        [Test]
        public void AsyncApiReference_WithExternalFragmentUriReference_AllowReference()
        {
            // Arrange
            var actual = """
                payload:
                  $ref: http://example.com/some-resource#/path/to/external/fragment
                """;
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaPayload>();
            payload.UnresolvedReference.Should().BeTrue();

            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("http://example.com/some-resource");
            reference.Id.Should().Be("/path/to/external/fragment");
            reference.IsFragment.Should().BeTrue();
            reference.IsExternal.Should().BeTrue();
            reference.Type.Should().Be(ReferenceType.Schema);
            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiReference_WithFragmentReference_AllowReference()
        {
            // Arrange
            var actual = """
                payload:
                  $ref: /fragments/myFragment
                """;
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaPayload>();
            payload.UnresolvedReference.Should().BeTrue();

            var reference = payload.Reference;
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.ExternalResource.Should().Be("/fragments/myFragment");
            reference.Id.Should().BeNull();
            reference.IsFragment.Should().BeFalse();
            reference.IsExternal.Should().BeTrue();
            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiReference_WithInternalComponentReference_AllowReference()
        {
            // Arrange
            var actual = """
                payload:
                  $ref: '#/components/schemas/test'
                """;
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaPayload>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().BeNull();
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.Id.Should().Be("test");
            reference.IsFragment.Should().BeFalse();
            reference.IsExternal.Should().BeFalse();

            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiReference_WithExternalFragmentReference_AllowReference()
        {
            // Arrange
            var actual = """
                payload:
                  $ref: ./myjsonfile.json#/fragment
                """;
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaPayload>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("./myjsonfile.json");
            reference.Id.Should().Be("/fragment");
            reference.IsFragment.Should().BeTrue();
            reference.IsExternal.Should().BeTrue();

            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiReference_WithExternalComponentReference_AllowReference()
        {
            // Arrange
            var actual = """
                payload:
                  $ref: ./someotherdocument.json#/components/schemas/test
                """;
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaPayload>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("./someotherdocument.json");
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.Id.Should().Be("test");
            reference.IsFragment.Should().BeFalse();
            reference.IsExternal.Should().BeTrue();

            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiDocument_WithInternalComponentReference_ResolvesReference()
        {
            // Arrange
            var actual = """
                asyncapi: 2.6.0
                info:
                  title: My AsyncAPI Document
                  version: 1.0.0
                channels:
                  myChannel:
                    $ref: '#/components/channels/myChannel' 
                components:
                  channels: 
                    myChannel:
                      description: customDescription
                """;

            var settings = new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveInternalReferences,
            };
            var reader = new AsyncApiStringReader(settings);

            // Act
            var deserialized = reader.Read(actual, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var channel = deserialized.Channels.First().Value;

            channel.UnresolvedReference.Should().BeFalse();
            channel.Description.Should().Be("customDescription");
            channel.Reference.ExternalResource.Should().BeNull();
            channel.Reference.Id.Should().Be("myChannel");
            channel.Reference.IsExternal.Should().BeFalse();
            channel.Reference.Type.Should().Be(ReferenceType.Channel);
        }

        [Test]
        public void AsyncApiDocument_WithNoConfiguredExternalReferenceReader_ThrowsError()
        {
            // Arrange
            var actual = """
                asyncapi: 2.6.0
                info:
                  title: My AsyncAPI Document
                  version: 1.0.0
                channels:
                  myChannel:
                    $ref: http://example.com/channel.json
                """;

            var settings = new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveAllReferences,
            };
            var reader = new AsyncApiStringReader(settings);

            // Act
            reader.Read(actual, out var diagnostic);

            // Assert
            diagnostic.Errors.Count.Should().Be(1);
            var error = diagnostic.Errors.First();
            error.Message.Should()
                .Be(
                    "External reference configured in AsyncApi document but no implementation provided for ExternalReferenceReader.");
        }

        [Test]
        public void AsyncApiDocument_WithExternalReferenceOnlySetToResolveInternalReferences_DoesNotResolve()
        {
            // Arrange
            var actual = """
                         asyncapi: 2.6.0
                         info:
                           title: My AsyncAPI Document
                           version: 1.0.0
                         channels:
                           myChannel:
                             $ref: http://example.com/channel.json
                         """;

            var settings = new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveInternalReferences,
            };
            var reader = new AsyncApiStringReader(settings);

            // Act
            var deserialized = reader.Read(actual, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var channel = deserialized.Channels.First().Value;

            channel.UnresolvedReference.Should().BeTrue();
            channel.Description.Should().BeNull();
            channel.Reference.ExternalResource.Should().Be("http://example.com/channel.json");
            channel.Reference.Type.Should().Be(ReferenceType.Channel);
            channel.Reference.Id.Should().BeNull();
            channel.Reference.IsExternal.Should().BeTrue();
            channel.Reference.IsFragment.Should().BeFalse();
        }

        [Test]
        public void AsyncApiReference_WithExternalReference_AllowsReferenceDoesNotResolve()
        {
            // Arrange
            var actual = """
                payload:
                  $ref: http://example.com/json.json
                """;
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaPayload>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("http://example.com/json.json");
            reference.Id.Should().BeNull();
            reference.IsExternal.Should().BeTrue();
            reference.IsFragment.Should().BeFalse();
            diagnostic.Errors.Should().BeEmpty();

            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            expected
                .Should()
                .BePlatformAgnosticEquivalentTo(actual);
        }

        [Test]
        public void AsyncApiReference_WithExternalResourcesInterface_DeserializesCorrectly()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       channels:
                         workspace:
                           publish:
                             message:
                               $ref: "./some/path/to/external/message.yaml"
                       """;
            var settings = new AsyncApiReaderSettings
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveAllReferences,
                ExternalReferenceLoader = new MockLoader(),
            };
            var reader = new AsyncApiStringReader(settings);
            var doc = reader.Read(yaml, out var diagnostic);
            var message = doc.Channels["workspace"].Publish.Message.First();
            message.Name.Should().Be("Test");
            var payload = message.Payload.As<AsyncApiJsonSchemaPayload>();
            payload.Properties.Count.Should().Be(3);
        }

        //[Test]
        //public void AvroReference_WithExternalResourcesInterface_DeserializesCorrectly()
        //{
        //    var yaml = """
        //       asyncapi: 2.3.0
        //       info:
        //         title: test
        //         version: 1.0.0
        //       channels:
        //         workspace:
        //           publish:
        //             message:
        //              schemaFormat: 'application/vnd.apache.avro+yaml;version=1.9.0'
        //              payload:
        //                $ref: 'path/to/user-create.avsc/#UserCreate'
        //       """;
        //    var settings = new AsyncApiReaderSettings
        //    {
        //        ReferenceResolution = ReferenceResolutionSetting.ResolveAllReferences,
        //        ExternalReferenceReader = new MockExternalAvroReferenceReader(),
        //    };
        //    var reader = new AsyncApiStringReader(settings);
        //    var doc = reader.Read(yaml, out var diagnostic);
        //    var payload = doc.Channels["workspace"].Publish.Message.First().Payload;
        //    payload.Should().BeAssignableTo(typeof(AsyncApiAvroSchemaPayload));
        //    var avro = payload as AsyncApiAvroSchemaPayload;
        //    avro.TryGetAs<AvroRecord>(out var record);
        //    record.Name.Should().Be("SomeEvent");
        //}
    }

    public class MockLoader : IStreamLoader
    {
        const string Message = 
            """
            name: Test
            title: Test message
            summary: Test.
            schemaFormat: application/schema+yaml;version=draft-07
            contentType: application/cloudevents+json
            payload:
              $ref: "./some/path/to/schema.yaml"
            """;

        public Stream Load(Uri uri)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(Message);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public Task<Stream> LoadAsync(Uri uri)
        {
            return Task.FromResult(this.Load(uri));
        }
    }
}