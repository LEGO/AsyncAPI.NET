namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Bindings.Http;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class AsyncApiMessage_Should
    {
            [Test]
            public void AsyncApiMessage_WithNoSchemaFormat_DeserializesToDefault()
            {
                // Arrange
                var expected =
@"payload:
  properties:
    propertyA:
      type:
        - string
        - 'null'";

                // Act
                var message = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

                // Assert
                diagnostic.Errors.Should().BeEmpty();
                message.SchemaFormat.Should().BeNull();
            }

            [Test]
            public void AsyncApiMessage_WithUnsupportedSchemaFormat_DeserializesWithError()
            {
                // Arrange
                var expected =
    @"payload:
  properties:
    propertyA:
      type:
        - string
        - 'null'
schemaFormat: application/vnd.apache.avro;version=1.9.0";

                // Act
                new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

                // Assert
                diagnostic.Errors.Should().HaveCount(1);
                diagnostic.Errors.First().Message.Should().StartWith("'application/vnd.apache.avro;version=1.9.0' is not a supported format");
            }

            [Test]
            public void AsyncApiMessage_WithNoSchemaFormat_DoesNotSerializeSchemaFormat()
            {
                // Arrange
                var expected =
    @"payload:
  properties:
    propertyA:
      type:
        - string
        - 'null'";

                var message = new AsyncApiMessage();
                message.Payload = new AsyncApiSchema()
                {
                    Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "propertyA", new AsyncApiSchema()
                        {
                            Type = new List<SchemaType> { SchemaType.String, SchemaType.Null },
                        }
                    },
                },
                };

                // Act
                var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

                actual = actual.MakeLineBreaksEnvironmentNeutral();
                expected = expected.MakeLineBreaksEnvironmentNeutral();

                var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

                // Assert
                Assert.AreEqual(actual, expected);
                message.Should().BeEquivalentTo(deserializedMessage);
            }

            [Test]
            public void AsyncApiMessage_WithSchemaFormat_Serializes()
            {
                // Arrange
                var expected =
    @"payload:
  properties:
    propertyA:
      type:
        - string
        - 'null'
schemaFormat: application/vnd.aai.asyncapi+json;version=2.6.0";

                var message = new AsyncApiMessage();
                message.SchemaFormat = "application/vnd.aai.asyncapi+json;version=2.6.0";
                message.Payload = new AsyncApiSchema()
                {
                    Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "propertyA", new AsyncApiSchema()
                        {
                            Type = new List<SchemaType> { SchemaType.String, SchemaType.Null },
                        }
                    },
                },
                };

                // Act
                var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

                actual = actual.MakeLineBreaksEnvironmentNeutral();
                expected = expected.MakeLineBreaksEnvironmentNeutral();

                var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

                // Assert
                Assert.AreEqual(actual, expected);
                message.Should().BeEquivalentTo(deserializedMessage);
            }

            [Test]
        public void AsyncApiMessage_WithFilledObject_Serializes()
        {
            var expected =
@"headers:
  title: HeaderTitle
  description: HeaderDescription
  writeOnly: true
  examples:
    - x-correlation-id: nil
payload:
  properties:
    propA:
      type: string
    propB:
      type: string
correlationId:
  description: CorrelationDescription
  location: Header
  x-extension-a: a
contentType: MessageContentType
name: MessageName
title: MessageTitle
summary: MessageSummary
description: MessageDescription
tags:
  - name: tagA
    description: a
externalDocs:
  description: example docs description
  url: https://example.com/docs
bindings:
  http:
    headers:
      title: SchemaTitle
      description: SchemaDescription
      writeOnly: true
      examples:
        - cKey: c
          dKey: 1
examples:
  - payload:
      PropA: a
      PropB: b
traits:
  - headers:
      title: SchemaTitle
      description: SchemaDescription
      writeOnly: true
      examples:
        - eKey: e
          fKey: 1
    name: MessageTraitName
    title: MessageTraitTitle
    summary: MessageTraitSummary
    description: MessageTraitDescription
    tags:
      - name: tagB
        description: b
    externalDocs:
      description: example docs description
      url: https://example.com/docs
    examples:
      - name: MessageExampleName
        summary: MessageExampleSummary
        payload:
          gKey: g
          hKey: true
        x-extension-b: b
    x-extension-c: c";

            var message = new AsyncApiMessage
            {
                Headers = new AsyncApiSchema
                {
                    Title = "HeaderTitle",
                    WriteOnly = true,
                    Description = "HeaderDescription",
                    Examples = new List<IAsyncApiAny>
                    {
                        new AsyncApiObject
                        {
                            { "x-correlation-id", new AsyncApiString("nil") },
                        },
                    },
                },
                Payload = new AsyncApiSchema()
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        {
                            "propA", new AsyncApiSchema()
                            {
                                Type = new List<SchemaType>()
                                {
                                    SchemaType.String,
                                },
                            }
                        },
                        {
                            "propB", new AsyncApiSchema()
                            {
                                Type = new List<SchemaType>()
                                {
                                    SchemaType.String,
                                },
                            }
                        },
                    },
                },
                CorrelationId = new AsyncApiCorrelationId
                {
                    Location = "Header",
                    Description = "CorrelationDescription",
                    Extensions = new Dictionary<string, IAsyncApiExtension>
                    {
                        { "x-extension-a", new AsyncApiString("a") },
                    },
                },
                ContentType = "MessageContentType",
                Name = "MessageName",
                Title = "MessageTitle",
                Summary = "MessageSummary",
                Description = "MessageDescription",
                Tags = new List<AsyncApiTag>
                {
                    new AsyncApiTag
                    {
                        Name = "tagA",
                        Description = "a",
                    },
                },
                ExternalDocs = new AsyncApiExternalDocumentation
                {
                    Url = new Uri("https://example.com/docs"),
                    Description = "example docs description",
                },
                Bindings = new AsyncApiBindings<IMessageBinding>()
                {
                    {
                        "http", new HttpMessageBinding
                        {
                            Headers = new AsyncApiSchema
                            {
                                Title = "SchemaTitle",
                                WriteOnly = true,
                                Description = "SchemaDescription",
                                Examples = new List<IAsyncApiAny>
                                {
                                    new AsyncApiObject
                                    {
                                        { "cKey", new AsyncApiString("c") },
                                        { "dKey", new AsyncApiInteger(1) },
                                    },
                                },
                            },
                        }
                    },
                },
                Examples = new List<AsyncApiMessageExample>
                {
                    new AsyncApiMessageExample
                    {
                        Payload = new AsyncApiObject()
                        {
                            { "PropA", new AsyncApiString("a") },
                            { "PropB", new AsyncApiString("b") },
                        },
                    },
                },
                Traits = new List<AsyncApiMessageTrait>
                {
                    new AsyncApiMessageTrait
                    {
                        Name = "MessageTraitName",
                        Title = "MessageTraitTitle",
                        Headers = new AsyncApiSchema
                        {
                            Title = "SchemaTitle",
                            WriteOnly = true,
                            Description = "SchemaDescription",
                            Examples = new List<IAsyncApiAny>
                            {
                                new AsyncApiObject
                                {
                                    { "eKey", new AsyncApiString("e") },
                                    { "fKey", new AsyncApiInteger(1) },
                                },
                            },
                        },
                        Examples = new List<AsyncApiMessageExample>
                        {
                            new AsyncApiMessageExample
                            {
                                Summary = "MessageExampleSummary",
                                Name = "MessageExampleName",
                                Payload = new AsyncApiObject
                                {
                                    { "gKey", new AsyncApiString("g") },
                                    { "hKey", new AsyncApiBoolean(true) },
                                },
                                Extensions = new Dictionary<string, IAsyncApiExtension>
                                {
                                    { "x-extension-b", new AsyncApiString("b") },
                                },
                            },
                        },
                        Description = "MessageTraitDescription",
                        Summary = "MessageTraitSummary",
                        Tags = new List<AsyncApiTag>
                        {
                            new AsyncApiTag
                            {
                                Name = "tagB",
                                Description = "b",
                            },
                        },
                        ExternalDocs = new AsyncApiExternalDocumentation
                        {
                            Url = new Uri("https://example.com/docs"),
                            Description = "example docs description",
                        },
                        Extensions = new Dictionary<string, IAsyncApiExtension>
                        {
                            { "x-extension-c", new AsyncApiString("c") },
                        },
                    },
                },
            };

            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            message.Should().BeEquivalentTo(deserializedMessage);
        }
    }
}
