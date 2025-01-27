// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class AsyncApiSchema_Should : TestBase
    {
        public static AsyncApiJsonSchema BasicSchema = new AsyncApiJsonSchema();

        public static AsyncApiJsonSchema AdvancedSchemaNumber = new AsyncApiJsonSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = 42,
            ExclusiveMinimum = 42,
            Minimum = 10,
            Default = new AsyncApiAny(15),
            Type = SchemaType.Integer,
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        public static AsyncApiJsonSchema AdvancedSchemaBigNumbers = new AsyncApiJsonSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = double.MaxValue,
            ExclusiveMinimum = double.MinValue,
            Minimum = double.MinValue,
            Default = new AsyncApiAny(15),
            Type = SchemaType.Integer,
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        public static AsyncApiJsonSchema AdvancedSchemaObject = new AsyncApiJsonSchema
        {
            Title = "title1",
            Properties = new Dictionary<string, AsyncApiJsonSchema>
            {
                ["property1"] = new AsyncApiJsonSchema
                {
                    Properties = new Dictionary<string, AsyncApiJsonSchema>
                    {
                        ["property2"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.Integer,
                        },
                        ["property3"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.String,
                            MaxLength = 15,
                        },
                    },
                    AdditionalProperties = new FalseApiSchema(),
                    Items = new FalseApiSchema(),
                    AdditionalItems = new FalseApiSchema(),
                },
                ["property4"] = new AsyncApiJsonSchema
                {
                    Properties = new Dictionary<string, AsyncApiJsonSchema>
                    {
                        ["property5"] = new AsyncApiJsonSchema
                        {
                            Properties = new Dictionary<string, AsyncApiJsonSchema>
                            {
                                ["property6"] = new AsyncApiJsonSchema
                                {
                                    Type = SchemaType.Boolean,
                                },
                            },
                        },
                        ["property7"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.String,
                            MinLength = 2,
                        },
                    },
                    PatternProperties = new Dictionary<string, AsyncApiJsonSchema>()
                    {
                        {
                            "^S_",
                            new AsyncApiJsonSchema()
                            {
                                Type = SchemaType.String,
                            }
                        },
                        {
                            "^I_", new AsyncApiJsonSchema()
                            {
                                Type = SchemaType.Integer,
                            }
                        },
                    },
                    PropertyNames = new AsyncApiJsonSchema()
                    {
                        Pattern = "^[A-Za-z_][A-Za-z0-9_]*$",
                    },
                    AdditionalProperties = new AsyncApiJsonSchema
                    {
                        Properties = new Dictionary<string, AsyncApiJsonSchema>
                        {
                            ["Property8"] = new AsyncApiJsonSchema
                            {
                                Type = SchemaType.String | SchemaType.Null,
                            },
                        },
                    },
                    Items = new AsyncApiJsonSchema
                    {
                        Properties = new Dictionary<string, AsyncApiJsonSchema>
                        {
                            ["Property9"] = new AsyncApiJsonSchema
                            {
                                Type = SchemaType.String | SchemaType.Null,
                            },
                        },
                    },
                    AdditionalItems = new AsyncApiJsonSchema
                    {
                        Properties = new Dictionary<string, AsyncApiJsonSchema>
                        {
                            ["Property10"] = new AsyncApiJsonSchema
                            {
                                Type = SchemaType.String | SchemaType.Null,
                            },
                        },
                    },
                },
                ["property11"] = new AsyncApiJsonSchema
                {
                    Const = new AsyncApiAny("aSpecialConstant"),
                },
            },
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        public static AsyncApiJsonSchema AdvancedSchemaWithAllOf = new AsyncApiJsonSchema
        {
            Title = "title1",
            AllOf = new List<AsyncApiJsonSchema>
            {
                new AsyncApiJsonSchema
                {
                    Title = "title2",
                    Properties = new Dictionary<string, AsyncApiJsonSchema>
                    {
                        ["property1"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.Integer,
                        },
                        ["property2"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.String,
                            MaxLength = 15,
                        },
                    },
                },
                new AsyncApiJsonSchema
                {
                    Title = "title3",
                    Properties = new Dictionary<string, AsyncApiJsonSchema>
                    {
                        ["property3"] = new AsyncApiJsonSchema
                        {
                            Properties = new Dictionary<string, AsyncApiJsonSchema>
                            {
                                ["property4"] = new AsyncApiJsonSchema
                                {
                                    Type = SchemaType.Boolean ,
                                },
                            },
                        },
                        ["property5"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.String,
                            MinLength = 2,
                        },
                    },
                    Nullable = true,
                },
            },
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        public static AsyncApiJsonSchema ReferencedSchema = new AsyncApiJsonSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = 42,
            ExclusiveMinimum = 42,
            Minimum = 10,
            Default = new AsyncApiAny(15),
            Type = SchemaType.Integer,

            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },

            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Schema,
                FragmentId = "schemaObject1",
            },
        };

        public static AsyncApiJsonSchema AdvancedSchemaWithRequiredPropertiesObject = new AsyncApiJsonSchema
        {
            Title = "title1",
            Required = new HashSet<string>() { "property1" },
            Properties = new Dictionary<string, AsyncApiJsonSchema>
            {
                ["property1"] = new AsyncApiJsonSchema
                {
                    Required = new HashSet<string>() { "property3" },
                    Properties = new Dictionary<string, AsyncApiJsonSchema>
                    {
                        ["property2"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.Integer,
                        },
                        ["property3"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.String,
                            MaxLength = 15,
                            ReadOnly = true,
                        },
                    },
                    ReadOnly = true,
                },
                ["property4"] = new AsyncApiJsonSchema
                {
                    Properties = new Dictionary<string, AsyncApiJsonSchema>
                    {
                        ["property5"] = new AsyncApiJsonSchema
                        {
                            Properties = new Dictionary<string, AsyncApiJsonSchema>
                            {
                                ["property6"] = new AsyncApiJsonSchema
                                {
                                    Type = SchemaType.Boolean,
                                },
                            },
                        },
                        ["property7"] = new AsyncApiJsonSchema
                        {
                            Type = SchemaType.String,
                            MinLength = 2,
                        },
                    },
                    ReadOnly = true,
                },
            },
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        [Test]
        public void SerializeAsJson_WithBasicSchema_V2Works()
        {
            // Arrange
            var expected = @"{ }";

            // Act
            var actual = BasicSchema.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaNumber_V2Works()
        {
            // Arrange
            var expected = """
                {
                  "title": "title1",
                  "type": "integer",
                  "maximum": 42,
                  "minimum": 10,
                  "exclusiveMinimum": 42,
                  "multipleOf": 3,
                  "default": 15,
                  "nullable": true,
                  "externalDocs": {
                    "url": "http://example.com/externalDocs"
                  }
                }
                """;

            // Act
            var actual = AdvancedSchemaNumber.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaBigNumbers_V2Works()
        {
            // Arrange
            var expected = """
                {
                  "title": "title1",
                  "type": "integer",
                  "maximum": 1.7976931348623157E+308,
                  "minimum": -1.7976931348623157E+308,
                  "exclusiveMinimum": -1.7976931348623157E+308,
                  "multipleOf": 3,
                  "default": 15,
                  "nullable": true,
                  "externalDocs": {
                    "url": "http://example.com/externalDocs"
                  }
                }
                """;

            // Act
            var actual = AdvancedSchemaBigNumbers.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaObject_V2Works()
        {
            // Arrange
            string expected = this.GetTestData<string>();

            // Act
            var actual = AdvancedSchemaObject.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void Deserialize_WithAdvancedSchema_Works()
        {
            // Arrange
            var json = this.GetTestData<string>();
            var expected = AdvancedSchemaObject;

            // Act
            var actual = new AsyncApiStringReader().ReadFragment<AsyncApiJsonSchema>(json, AsyncApiVersion.AsyncApi2_0, out var _diagnostics);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaWithAllOf_V2Works()
        {
            // Arrange
            var expected = this.GetTestData<string>();

            // Act
            var actual = AdvancedSchemaWithAllOf.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void Serialize_WithInliningOptions_ShouldInlineAccordingly(bool shouldInline)
        {
            // arrange
            var asyncApiDocument = new AsyncApiDocumentBuilder()
            .WithInfo(new AsyncApiInfo
            {
                Title = "Streetlights Kafka API",
                Version = "1.0.0",
                Description = "The Smartylighting Streetlights API allows you to remotely manage the city lights.",
                License = new AsyncApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                },
            })
            .WithChannel("mychannel", new AsyncApiChannel()
            {
                Publish = new AsyncApiOperation
                {
                    Message = new List<AsyncApiMessage>
                    {
                        new AsyncApiMessage
                        {
                            Payload = new AsyncApiJsonSchemaPayload
                            {
                                Type = SchemaType.Object,
                                Required = new HashSet<string> { "testB" },
                                Properties = new Dictionary<string, AsyncApiJsonSchema>
                                {
                                    { "testC", new AsyncApiJsonSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, FragmentId = "testC" } } },
                                    { "testB", new AsyncApiJsonSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, FragmentId = "testB" } } },
                                },
                            },
                        },
                    },
                },
            })
            .WithComponent("testD", new AsyncApiJsonSchema() { Type = SchemaType.String, Format = "uuid" })
            .WithComponent("testC", new AsyncApiJsonSchema()
            {
                Type = SchemaType.Object,
                Properties = new Dictionary<string, AsyncApiJsonSchema>
                {
                    { "testD", new AsyncApiJsonSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, FragmentId = "testD" } } },
                },
            })
            .WithComponent("testB", new AsyncApiJsonSchema() { Description = "test", Type = SchemaType.Boolean })
            .Build();

            var outputString = new StringWriter();
            var writer = new AsyncApiYamlWriter(outputString, new AsyncApiWriterSettings { InlineLocalReferences = shouldInline });

            // Act
            asyncApiDocument.SerializeV2(writer);

            var actual = outputString.ToString();

            // Assert
            string expected = this.GetTestData<string>(shouldInline
                ? "AsyncApiSchema_InlinedReferences"
                : "AsyncApiSchema_NoInlinedReferences.yml");

            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiLicense = new AsyncApiLicense();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiLicense.SerializeV2(null); });
        }

        /// <summary>
        /// Regression test.
        /// Bug: Serializing properties multiple times - specifically Schema.OneOf was serialized into OneOf and Then.
        /// </summary>
        [Test]
        public void Serialize_WithOneOf_DoesNotWriteThen()
        {
            var mainSchema = new AsyncApiJsonSchema();
            var subSchema = new AsyncApiJsonSchema();
            subSchema.Properties.Add("title", new AsyncApiJsonSchema() { Type = SchemaType.String });
            mainSchema.OneOf = new List<AsyncApiJsonSchema>() { subSchema };

            var yaml = mainSchema.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            Assert.True(!yaml.Contains("then:"), "then");
            Assert.True(yaml.Contains("oneOf:"), "oneOf");
        }

        /// <summary>
        /// Regression test.
        /// Bug: Serializing properties multiple times - specifically Schema.AnyOf was serialized into AnyOf and If.
        /// </summary>
        [Test]
        public void Serialize_WithAnyOf_DoesNotWriteIf()
        {
            var mainSchema = new AsyncApiJsonSchema();
            var subSchema = new AsyncApiJsonSchema();
            subSchema.Properties.Add("title", new AsyncApiJsonSchema() { Type = SchemaType.String });
            mainSchema.AnyOf = new List<AsyncApiJsonSchema>() { subSchema };

            var yaml = mainSchema.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            Assert.True(!yaml.Contains("if:"));
        }

        [Test]
        public void Deserialize_BasicExample()
        {
            var input =
                """
                title: title1
                type: integer
                maximum: 42
                minimum: 10
                exclusiveMinimum: 42.0
                multipleOf: 3
                default: 15
                nullable: true
                externalDocs:
                  url: http://example.com/externalDocs
                """;

            var schema = new AsyncApiStringReader().ReadFragment<AsyncApiJsonSchema>(input, AsyncApiVersion.AsyncApi2_0, out var diag);

            diag.Errors.Should().BeEmpty();
            schema.Should().BeEquivalentTo(AdvancedSchemaNumber);
        }
        /// <summary>
        /// Regression test.
        /// Bug: Serializing properties multiple times - specifically Schema.Not was serialized into Not and Else.
        /// </summary>
        [Test]
        public void Serialize_WithNot_DoesNotWriteElse()
        {
            var mainSchema = new AsyncApiJsonSchema();
            var subSchema = new AsyncApiJsonSchema();
            subSchema.Properties.Add("title", new AsyncApiJsonSchema() { Type = SchemaType.String });
            mainSchema.Not = subSchema;

            var yaml = mainSchema.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            Assert.True(!yaml.Contains("else:"));
        }
    }
}
