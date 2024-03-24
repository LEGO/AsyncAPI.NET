// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using FluentAssertions;
    using FluentAssertions.Primitives;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;
    using System.Linq;

    public class AsyncApiReference_Should : TestBase
    {

        [Test]
        public void AsyncApiReference_WithExternalFragmentUriReference_AllowReference()
        {
            // Arrange
            var actual = @"payload:
  $ref: http://example.com/some-resource#/path/to/external/fragment";
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            deserialized.Payload.UnresolvedReference.Should().BeTrue();

            var reference = deserialized.Payload.Reference;
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
            var actual = @"payload:
  $ref: /fragments/myFragment";
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            deserialized.Payload.UnresolvedReference.Should().BeTrue();

            var reference = deserialized.Payload.Reference;
            reference.Type.Should().Be(ReferenceType.Schema);
            reference.ExternalResource.Should().Be("/fragments/myFragment");
            reference.Id.Should().BeNull();
            reference.IsFragment.Should().BeTrue();
            reference.IsExternal.Should().BeTrue();
            var expected = deserialized.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiReference_WithInternalComponentReference_AllowReference()
        {
            // Arrange
            var actual = @"payload:
  $ref: '#/components/schemas/test'";
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
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
            var actual = @"payload:
  $ref: ./myjsonfile.json#/fragment";
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
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
            var actual = @"payload:
  $ref: ./someotherdocument.json#/components/schemas/test";
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
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
            var actual = @"asyncapi: 2.6.0
info:
  title: My AsyncAPI Document
  version: 1.0.0
channels:
  myChannel:
    $ref: '#/components/channels/myChannel' 
components:
  channels: 
    myChannel:
      description: customDescription";

            var settings = new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveReferences,
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
        public void AsyncApiDocument_WithExternalReference_DoesNotResolve()
        {
            // Arrange
            var actual = @"asyncapi: 2.6.0
info:
  title: My AsyncAPI Document
  version: 1.0.0
channels:
  myChannel:
    $ref: http://example.com/channel.json";

            var settings = new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveReferences,
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
            var actual = @"payload:
  $ref: http://example.com/json.json";
            var reader = new AsyncApiStringReader();

            // Act
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
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
    }
}