// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Http;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class AsyncApiMessage_Should : TestBase
    {
        [Test]
        public void AsyncApiMessage_WithNoType_DeserializesToDefault()
        {
            // Arrange
            var expected =
                """
                {
                                      "payload": {
                                        "type": "object",
                                        "properties": {
                                          "someProp": {
                                            "enum": [
                                              "test",
                                              "test2"
                                            ]
                                          }
                                        }
                                      }
                                    }
                """;

            // Act
            var message = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().BeEmpty();
            message.Payload.As<AsyncApiJsonSchemaPayload>().Properties.First().Value.Enum.Should().HaveCount(2);
        }

        [Test]
        public void AsyncApiMessage_WithNoSchemaFormat_DeserializesToDefault()
        {
            // Arrange
            var expected =
                """
                payload:
                  properties:
                    propertyA:
                      type:
                        - 'null'
                        - string
                """;

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
                """
                payload:
                  properties:
                    propertyA:
                      type:
                        - 'null'
                        - string
                schemaFormat: whatever
                """;

            // Act
            new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            diagnostic.Errors.Should().HaveCount(1);
            diagnostic.Errors.First().Message.Should().StartWith("'whatever' is not a supported format");
        }

        [Test]
        public void AsyncApiMessage_WithNoSchemaFormat_DoesNotSerializeSchemaFormat()
        {
            // Arrange
            var expected =
                """
                payload:
                  properties:
                    propertyA:
                      type:
                        - 'null'
                        - string
                """;

            var message = new AsyncApiMessage();
            message.Payload = new AsyncApiJsonSchemaPayload()
            {
                Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "propertyA", new AsyncApiSchema()
                        {
                            Type = SchemaType.String | SchemaType.Null,
                        }
                    },
                },
            };

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            message.Should().BeEquivalentTo(deserializedMessage);
            }

        [Test]
        public void AsyncApiMessage_WithJsonSchemaFormat_Serializes()
        {
            // Arrange
            var expected =
                """
                payload:
                  properties:
                    propertyA:
                      type:
                        - 'null'
                        - string
                schemaFormat: application/vnd.aai.asyncapi+json;version=2.6.0
                """;

            var message = new AsyncApiMessage();
            message.SchemaFormat = "application/vnd.aai.asyncapi+json;version=2.6.0";
            message.Payload = new AsyncApiJsonSchemaPayload()
            {
                Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "propertyA", new AsyncApiSchema()
                        {
                            Type = SchemaType.String | SchemaType.Null,
                        }
                    },
                },
            };

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            message.Should().BeEquivalentTo(deserializedMessage);
        }

        [Test]
        public void AsyncApiMessage_WithAvroSchemaFormat_Serializes()
        {
            // Arrange
            var expected =
            """
            payload:
              type: record
              name: User
              namespace: com.example
              fields:
                - name: username
                  type: string
                  doc: The username of the user.
                  default: guest
                  order: ascending
            schemaFormat: application/vnd.apache.avro
            """;

            var message = new AsyncApiMessage();
            message.SchemaFormat = "application/vnd.apache.avro";
            message.Payload = new AsyncApiAvroSchemaPayload()
            {
                Schema = new AvroRecord()
                {
                    Name = "User",
                    Namespace = "com.example",
                    Fields = new List<AvroField>
                    {
                        new AvroField()
                        {
                            Name = "username",
                            Type = AvroPrimitiveType.String,
                            Doc = "The username of the user.",
                            Default = new AsyncApiAny("guest"),
                            Order = AvroFieldOrder.Ascending,
                        },
                    },
                },
            };

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            message.Should().BeEquivalentTo(deserializedMessage);
        }

        [Test]
        public void AsyncApiMessage_WithAvroAsReference_Deserializes()
        {
            // Arrange
            var input =
            """
            schemaFormat: 'application/vnd.apache.avro+yaml;version=1.9.0'
            payload:
              $ref: 'path/to/user-create.avsc/#UserCreate'
            """;

            // Act
            var deserializedMessage = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            deserializedMessage.Payload.UnresolvedReference.Should().BeTrue();
            deserializedMessage.Payload.Reference.Should().NotBeNull();
            deserializedMessage.Payload.Reference.IsExternal.Should().BeTrue();
            deserializedMessage.Payload.Reference.IsFragment.Should().BeTrue();

        }

        [Test]
        public void AsyncApiMessage_WithFilledObject_Serializes()
        {
            var expected =
                """
                headers:
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
                    x-extension-c: c
                """;

            var message = new AsyncApiMessage
            {
                Headers = new AsyncApiSchema
                {
                    Title = "HeaderTitle",
                    WriteOnly = true,
                    Description = "HeaderDescription",
                    Examples = new List<AsyncApiAny>
                    {
                        new AsyncApiAny(new Dictionary<string, string>
                        {
                            { "x-correlation-id", "nil" },
                        }),
                    },
                },
                Payload = new AsyncApiJsonSchemaPayload()
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        {
                            "propA", new AsyncApiSchema()
                            {
                                Type = SchemaType.String,
                            }
                        },
                        {
                            "propB", new AsyncApiSchema()
                            {
                                Type = SchemaType.String,
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
                        { "x-extension-a", new AsyncApiAny("a") },
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
                                Examples = new List<AsyncApiAny>
                                {
                                    new AsyncApiAny(new Dictionary<string, object>
                                    {
                                        { "cKey", "c" },
                                        { "dKey", 1 },
                                    }),
                                },
                            },
                        }
                    },
                },
                Examples = new List<AsyncApiMessageExample>
                {
                    new AsyncApiMessageExample
                    {
                        Payload = new AsyncApiAny(new Dictionary<string, string>()
                        {
                            { "PropA", "a" },
                            { "PropB", "b" },
                        }),
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
                            Examples = new List<AsyncApiAny>
                            {
                                new AsyncApiAny(new Dictionary<string, object>
                                {
                                    { "eKey", "e" },
                                    { "fKey", 1 },
                                }),
                            },
                        },
                        Examples = new List<AsyncApiMessageExample>
                        {
                            new AsyncApiMessageExample
                            {
                                Summary = "MessageExampleSummary",
                                Name = "MessageExampleName",
                                Payload = new AsyncApiAny(new Dictionary<string, object>
                                {
                                    { "gKey", "g" },
                                    { "hKey", true },
                                }),
                                Extensions = new Dictionary<string, IAsyncApiExtension>
                                {
                                    { "x-extension-b", new AsyncApiAny("b") },
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
                            { "x-extension-c", new AsyncApiAny("c") },
                        },
                    },
                },
            };

            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.All;
            var deserializedMessage = new AsyncApiStringReader(settings).ReadFragment<AsyncApiMessage>(expected, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            message.Should().BeEquivalentTo(deserializedMessage);
        }
    }
}
