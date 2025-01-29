// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.V2;
    using NUnit.Framework;

    public class AsyncApiReference_Should : TestBase
    {
        [Test]
        public void ReferencePointers()
        {
            var diag = new AsyncApiDiagnostic();
            var versionService = new AsyncApiV2VersionService(diag);
            var externalFragment = versionService.ConvertToAsyncApiReference("https://github.com/test/test#whatever", ReferenceType.None);
            var internalFragment = versionService.ConvertToAsyncApiReference("#/components/servers/server1", ReferenceType.None);
            var localFile = versionService.ConvertToAsyncApiReference("./local/some/folder/whatever.yaml", ReferenceType.None);
            var externalFile = versionService.ConvertToAsyncApiReference("https://github.com/test/whatever.yaml", ReferenceType.None);

            externalFragment.ExternalResource.Should().Be("https://github.com/test/test");
            externalFragment.FragmentId.Should().Be("whatever");
            externalFragment.Reference.Should().Be("https://github.com/test/test#whatever");
            externalFragment.IsFragment.Should().BeTrue();
            externalFragment.IsExternal.Should().BeTrue();

            internalFragment.ExternalResource.Should().BeNull();
            internalFragment.FragmentId.Should().Be("/components/servers/server1");
            internalFragment.Reference.Should().Be("#/components/servers/server1");
            internalFragment.IsFragment.Should().BeTrue();
            internalFragment.IsExternal.Should().BeFalse();

            localFile.ExternalResource.Should().Be("./local/some/folder/whatever.yaml");
            localFile.Reference.Should().Be("./local/some/folder/whatever.yaml");
            localFile.FragmentId.Should().Be(null);
            localFile.IsFragment.Should().BeFalse();

            externalFile.ExternalResource.Should().Be("https://github.com/test/whatever.yaml");
            externalFile.Reference.Should().Be("https://github.com/test/whatever.yaml");
            externalFile.FragmentId.Should().Be(null);
            externalFile.IsFragment.Should().BeFalse();
        }

        [Test]
        public void Reference()
        {
            var json =
                """
        {
          "asyncapi": "2.6.0",
          "info": { },
          "servers": {
            "production": {
              "$ref": "https://github.com/test/test#whatever"
            }
          }
        }
        """;

            var doc = new AsyncApiStringReader().Read(json, out var diag);
            var reference = doc.Servers.First().Value as AsyncApiServerReference;
            reference.Reference.ExternalResource.Should().Be("https://github.com/test/test");
            reference.Reference.FragmentId.Should().Be("whatever");
            reference.Reference.IsFragment.Should().BeTrue();
        }

        [Test]
        public void ResolveExternalReference()
        {
            var json =
                """
 {
   "asyncapi": "2.6.0",
   "info": { },
   "servers": {
     "production": {
       "$ref": "https://gist.githubusercontent.com/VisualBean/7dc9607d735122483e1bb7005ff3ad0e/raw/458729e4d56636ef3bb34762f4a5731ea5043bdf/servers.json#/servers/0"
     }
   }
 }
 """;

            var doc = new AsyncApiStringReader(new AsyncApiReaderSettings { ReferenceResolution = ReferenceResolutionSetting.ResolveAllReferences }).Read(json, out var diag);
            var reference = doc.Servers.First().Value as AsyncApiServerReference;
            //reference.Reference.Id.Should().Be("whatever");
            //reference.Reference.HostDocument.Should().Be(doc);
            //reference.Reference.IsFragment.Should().BeTrue();
        }


        [Test]
        public void ServerReference_WithComponentReference_ResolvesReference()
        {
            var json =
                """
        {
          "asyncapi": "2.6.0",
          "info": { },
          "servers": {
            "production": {
              "$ref": "#/components/servers/whatever"
            }
          },
          "components": {
            "servers": {
                "whatever": {
                  "url": "wss://production.gigantic-server.com:443",
                  "protocol": "wss",
                  "protocolVersion": "1.0.0",
                  "description": "The production API server",
                  "variables": {
                    "username": {
                      "default": "demo",
                      "description": "This value is assigned by the service provider"
                    },
                    "password": {
                      "default": "demo",
                      "description": "This value is assigned by the service provider"
                    }
                  }
                }
            }
          }
        }
        """;

            var doc = new AsyncApiStringReader().Read(json, out var diag);
            var reference = doc.Servers.First().Value as AsyncApiServerReference;
            reference.Reference.ExternalResource.Should().BeNull();
            reference.Reference.FragmentId.Should().Be("/components/servers/whatever");
            reference.Reference.IsFragment.Should().BeTrue();
            reference.Url.Should().Be("wss://production.gigantic-server.com:443");

        }

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
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaReference>();
            payload.UnresolvedReference.Should().BeTrue();

            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("http://example.com/some-resource");
            reference.FragmentId.Should().Be("/path/to/external/fragment");
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
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaReference>();
            payload.UnresolvedReference.Should().BeTrue();

            var reference = payload.Reference;
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.ExternalResource.Should().Be("/fragments/myFragment");
            reference.FragmentId.Should().BeNull();
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
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaReference>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().BeNull();
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.FragmentId.Should().Be("/components/schemas/test");
            reference.IsFragment.Should().BeTrue();
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
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaReference>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("./myjsonfile.json");
            reference.FragmentId.Should().Be("/fragment");
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
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaReference>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("./someotherdocument.json");
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.FragmentId.Should().Be("/components/schemas/test");
            reference.IsFragment.Should().BeTrue();
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
            var channel = deserialized.Channels.First().Value as AsyncApiChannelReference;

            channel.UnresolvedReference.Should().BeFalse();
            channel.Description.Should().Be("customDescription");
            channel.Reference.ExternalResource.Should().BeNull();
            channel.Reference.FragmentId.Should().Be("/components/channels/myChannel");
            channel.Reference.IsExternal.Should().BeFalse();
            channel.Reference.Type.Should().Be(ReferenceType.Channel);
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
            var channel = deserialized.Channels.First().Value as AsyncApiChannelReference;

            channel.UnresolvedReference.Should().BeTrue();
            channel.Description.Should().BeNull();
            channel.Reference.ExternalResource.Should().Be("http://example.com/channel.json");
            channel.Reference.Type.Should().Be(ReferenceType.Channel);
            channel.Reference.FragmentId.Should().BeNull();
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
            var payload = deserialized.Payload.As<AsyncApiJsonSchemaReference>();
            var reference = payload.Reference;
            reference.ExternalResource.Should().Be("http://example.com/json.json");
            reference.FragmentId.Should().BeNull();
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
            var payload = message.Payload.As<AsyncApiJsonSchema>();
            payload.Properties.Count.Should().Be(1);
        }
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

        const string Schema =
            """
            type: object
            properties:
              lumens:
                type: integer
                minimum: 0
                description: Light intensity measured in lumens.
            """;

        public Stream Load(Uri uri)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            if (uri.ToString() == "./some/path/to/external/message.yaml")
            {
                writer.Write(Message);
            }
            else
            {
                writer.Write(Schema);
            }
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