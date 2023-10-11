// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    public class AsyncApiReference_Should
    {

        [Test]
        public void AsyncApiReference_WithExternalFragmentReference_AllowReference()
        {
            var actual = @"payload:
  $ref: 'http://example.com/some-resource#/path/to/external/fragment'
";
            var reader = new AsyncApiStringReader();
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
            reference.ExternalResource.Should().Be("http://example.com/some-resource");
            reference.Id.Should().Be("/path/to/external/fragment");
            reference.IsFragment.Should().BeTrue();
            reference.IsExternal.Should().BeTrue();
        }

        [Test]
        public void AsyncApiReference_WithFragmentReference_AllowReference()
        {
            var actual = @"payload:
  $ref: '/fragments/myFragment'
";
            var reader = new AsyncApiStringReader();
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
            reference.ExternalResource.Should().Be("/fragments/myFragment");
            reference.Id.Should().BeNull();
            reference.IsFragment.Should().BeTrue();
            reference.IsExternal.Should().BeTrue();
        }

        [Test]
        public void AsyncApiReference_WithExternalReference_AllowsReferenceDoesNotResolve()
        {
            var actual = @"payload:
  $ref: http://example.com/json.json
";

            var reader = new AsyncApiStringReader();
            var deserialized = reader.ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out var diagnostic);
            diagnostic.Errors.Should().BeEmpty();
            var reference = deserialized.Payload.Reference;
            reference.ExternalResource.Should().Be("http://example.com/json.json");
            reference.Id.Should().BeNull();
            reference.IsExternal.Should().BeTrue();
            diagnostic.Errors.Should().BeEmpty();
        }
    }
}