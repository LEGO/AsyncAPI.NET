﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class AsyncApiSchema_Should
    {
        public static AsyncApiSchema BasicSchema = new AsyncApiSchema();

        public static AsyncApiSchema AdvancedSchemaNumber = new AsyncApiSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = 42,
            ExclusiveMinimum = true,
            Minimum = 10,
            Default = new AsyncApiAny(15),
            Type = SchemaType.Integer,
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        public static AsyncApiSchema AdvancedSchemaBigNumbers = new AsyncApiSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = double.MaxValue,
            ExclusiveMinimum = true,
            Minimum = double.MinValue,
            Default = new AsyncApiAny(15),
            Type = SchemaType.Integer,
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocumentation
            {
                Url = new Uri("http://example.com/externalDocs"),
            },
        };

        public static AsyncApiSchema AdvancedSchemaObject = new AsyncApiSchema
        {
            Title = "title1",
            Properties = new Dictionary<string, AsyncApiSchema>
            {
                ["property1"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = SchemaType.Integer,
                        },
                        ["property3"] = new AsyncApiSchema
                        {
                            Type = SchemaType.String,
                            MaxLength = 15,
                        },
                    },
                    AdditionalProperties = new FalseApiSchema(),
                    Items = new FalseApiSchema(),
                    AdditionalItems = new FalseApiSchema(),
                },
                ["property4"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property5"] = new AsyncApiSchema
                        {
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["property6"] = new AsyncApiSchema
                                {
                                    Type = SchemaType.Boolean,
                                },
                            },
                        },
                        ["property7"] = new AsyncApiSchema
                        {
                            Type = SchemaType.String,
                            MinLength = 2,
                        },
                    },
                    PatternProperties = new Dictionary<string, AsyncApiSchema>()
                    {
                        {
                            "^S_",
                            new AsyncApiSchema()
                            {
                                Type = SchemaType.String,
                            }
                        },
                        {
                            "^I_", new AsyncApiSchema()
                            {
                                Type = SchemaType.Integer,
                            }
                        },
                    },
                    PropertyNames = new AsyncApiSchema()
                    {
                        Pattern = "^[A-Za-z_][A-Za-z0-9_]*$",
                    },
                    AdditionalProperties = new AsyncApiSchema
                    {
                        Properties = new Dictionary<string, AsyncApiSchema>
                        {
                            ["Property8"] = new AsyncApiSchema
                            {
                                Type = SchemaType.String | SchemaType.Null,
                            },
                        },
                    },
                    Items = new AsyncApiSchema
                    {
                        Properties = new Dictionary<string, AsyncApiSchema>
                        {
                            ["Property9"] = new AsyncApiSchema
                            {
                                Type = SchemaType.String | SchemaType.Null,
                            },
                        },
                    },
                    AdditionalItems = new AsyncApiSchema
                    {
                        Properties = new Dictionary<string, AsyncApiSchema>
                        {
                            ["Property10"] = new AsyncApiSchema
                            {
                                Type = SchemaType.String | SchemaType.Null,
                            },
                        },
                    },
                },
                ["property11"] = new AsyncApiSchema
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

        public static AsyncApiSchema AdvancedSchemaWithAllOf = new AsyncApiSchema
        {
            Title = "title1",
            AllOf = new List<AsyncApiSchema>
            {
                new AsyncApiSchema
                {
                    Title = "title2",
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property1"] = new AsyncApiSchema
                        {
                            Type = SchemaType.Integer,
                        },
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = SchemaType.String,
                            MaxLength = 15,
                        },
                    },
                },
                new AsyncApiSchema
                {
                    Title = "title3",
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property3"] = new AsyncApiSchema
                        {
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["property4"] = new AsyncApiSchema
                                {
                                    Type = SchemaType.Boolean ,
                                },
                            },
                        },
                        ["property5"] = new AsyncApiSchema
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

        public static AsyncApiSchema ReferencedSchema = new AsyncApiSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = 42,
            ExclusiveMinimum = true,
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
                Id = "schemaObject1",
            },
        };

        public static AsyncApiSchema AdvancedSchemaWithRequiredPropertiesObject = new AsyncApiSchema
        {
            Title = "title1",
            Required = new HashSet<string>() { "property1" },
            Properties = new Dictionary<string, AsyncApiSchema>
            {
                ["property1"] = new AsyncApiSchema
                {
                    Required = new HashSet<string>() { "property3" },
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = SchemaType.Integer,
                        },
                        ["property3"] = new AsyncApiSchema
                        {
                            Type = SchemaType.String,
                            MaxLength = 15,
                            ReadOnly = true,
                        },
                    },
                    ReadOnly = true,
                },
                ["property4"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property5"] = new AsyncApiSchema
                        {
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["property6"] = new AsyncApiSchema
                                {
                                    Type = SchemaType.Boolean,
                                },
                            },
                        },
                        ["property7"] = new AsyncApiSchema
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

        private string NoInlinedReferences =>
            @"asyncapi: '2.6.0'
info:
  title: Streetlights Kafka API
  version: 1.0.0
  description: The Smartylighting Streetlights API allows you to remotely manage the city lights.
  license:
    name: Apache 2.0
    url: https://www.apache.org/licenses/LICENSE-2.0
channels:
  mychannel:
    publish:
      message:
        payload:
          type: object
          required:
            - testB
          properties:
            testC:
              $ref: '#/components/schemas/testC'
            testB:
              $ref: '#/components/schemas/testB'
components:
  schemas:
    testD:
      type: string
      format: uuid
    testC:
      type: object
      properties:
        testD:
          $ref: '#/components/schemas/testD'
    testB:
      type: boolean
      description: test";

        private string InlinedReferences =>
            @"asyncapi: '2.6.0'
info:
  title: Streetlights Kafka API
  version: 1.0.0
  description: The Smartylighting Streetlights API allows you to remotely manage the city lights.
  license:
    name: Apache 2.0
    url: https://www.apache.org/licenses/LICENSE-2.0
channels:
  mychannel:
    publish:
      message:
        payload:
          type: object
          required:
            - testB
          properties:
            testC:
              type: object
              properties:
                testD:
                  type: string
                  format: uuid
            testB:
              type: boolean
              description: test
components: { }";

        [Test]
        public void SerializeAsJson_WithBasicSchema_V2Works()
        {
            // Arrange
            var expected = @"{ }";

            // Act
            var actual = BasicSchema.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaNumber_V2Works()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""type"": ""integer"",
  ""maximum"": 42,
  ""minimum"": 10,
  ""exclusiveMinimum"": true,
  ""multipleOf"": 3,
  ""default"": 15,
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaNumber.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaBigNumbers_V2Works()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""type"": ""integer"",
  ""maximum"": 1.7976931348623157E+308,
  ""minimum"": -1.7976931348623157E+308,
  ""exclusiveMinimum"": true,
  ""multipleOf"": 3,
  ""default"": 15,
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaBigNumbers.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaObject_V2Works()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""properties"": {
    ""property1"": {
      ""items"": false,
      ""additionalItems"": false,
      ""properties"": {
        ""property2"": {
          ""type"": ""integer""
        },
        ""property3"": {
          ""type"": ""string"",
          ""maxLength"": 15
        }
      },
      ""additionalProperties"": false
    },
    ""property4"": {
      ""items"": {
        ""properties"": {
          ""Property9"": {
            ""type"": [
              ""null"",
              ""string""
            ]
          }
        }
      },
      ""additionalItems"": {
        ""properties"": {
          ""Property10"": {
            ""type"": [
              ""null"",
              ""string""
            ]
          }
        }
      },
      ""properties"": {
        ""property5"": {
          ""properties"": {
            ""property6"": {
              ""type"": ""boolean""
            }
          }
        },
        ""property7"": {
          ""type"": ""string"",
          ""minLength"": 2
        }
      },
      ""additionalProperties"": {
        ""properties"": {
          ""Property8"": {
            ""type"": [
              ""null"",
              ""string""
            ]
          }
        }
      },
      ""patternProperties"": {
        ""^S_"": {
          ""type"": ""string""
        },
        ""^I_"": {
          ""type"": ""integer""
        }
      },
      ""propertyNames"": {
        ""pattern"": ""^[A-Za-z_][A-Za-z0-9_]*$""
      }
    },
    ""property11"": {
      ""const"": ""aSpecialConstant""
    }
  },
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaObject.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Test]
        public void Deserialize_WithAdvancedSchema_Works()
        {
            // Arrange
            var json = @"{
  ""title"": ""title1"",
  ""properties"": {
    ""property1"": {
      ""items"": false,
      ""additionalItems"": false,
      ""properties"": {
        ""property2"": {
          ""type"": ""integer""
        },
        ""property3"": {
          ""type"": ""string"",
          ""maxLength"": 15
        }
      },
      ""additionalProperties"": false
    },
    ""property4"": {
      ""items"": {
        ""properties"": {
          ""Property9"": {
            ""type"": [
              ""null"",
              ""string""
            ]
          }
        }
      },
      ""additionalItems"": {
        ""properties"": {
          ""Property10"": {
            ""type"": [
              ""null"",
              ""string""
            ]
          }
        }
      },
      ""properties"": {
        ""property5"": {
          ""properties"": {
            ""property6"": {
              ""type"": ""boolean""
            }
          }
        },
        ""property7"": {
          ""type"": ""string"",
          ""minLength"": 2
        }
      },
      ""additionalProperties"": {
        ""properties"": {
          ""Property8"": {
            ""type"": [
              ""null"",
              ""string""
            ]
          }
        }
      },
      ""patternProperties"": {
        ""^S_"": {
          ""type"": ""string""
        },
        ""^I_"": {
          ""type"": ""integer""
        }
      },
      ""propertyNames"": {
        ""pattern"": ""^[A-Za-z_][A-Za-z0-9_]*$""
      }
    },
    ""property11"": {
      ""const"": ""aSpecialConstant""
    }
  },
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";
            var expected = AdvancedSchemaObject;

            // Act
            var actual = new AsyncApiStringReader().ReadFragment<AsyncApiSchema>(json, AsyncApiVersion.AsyncApi2_0, out var _diagnostics);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void SerializeAsJson_WithAdvancedSchemaWithAllOf_V2Works()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""allOf"": [
    {
      ""title"": ""title2"",
      ""properties"": {
        ""property1"": {
          ""type"": ""integer""
        },
        ""property2"": {
          ""type"": ""string"",
          ""maxLength"": 15
        }
      }
    },
    {
      ""title"": ""title3"",
      ""properties"": {
        ""property3"": {
          ""properties"": {
            ""property4"": {
              ""type"": ""boolean""
            }
          }
        },
        ""property5"": {
          ""type"": ""string"",
          ""minLength"": 2
        }
      },
      ""nullable"": true
    }
  ],
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaWithAllOf.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
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
                            Payload = new AsyncApiSchema
                            {
                                Type = SchemaType.Object,
                                Required = new HashSet<string> { "testB" },
                                Properties = new Dictionary<string, AsyncApiSchema>
                                {
                                    { "testC", new AsyncApiSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, Id = "testC" } } },
                                    { "testB", new AsyncApiSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, Id = "testB" } } },
                                },
                            },
                        },
                    },
                },
            })
            .WithComponent("testD", new AsyncApiSchema() { Type = SchemaType.String, Format = "uuid" })
            .WithComponent("testC", new AsyncApiSchema()
            {
                Type = SchemaType.Object,
                Properties = new Dictionary<string, AsyncApiSchema>
                {
                    { "testD", new AsyncApiSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, Id = "testD" } } },
                },
            })
            .WithComponent("testB", new AsyncApiSchema() { Description = "test", Type = SchemaType.Boolean })
            .Build();

            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputString, new AsyncApiWriterSettings { InlineReferences = shouldInline });

            // Act
            asyncApiDocument.SerializeV2(writer);

            var actual = outputString.ToString();
            actual = actual.MakeLineBreaksEnvironmentNeutral();

            string expected = string.Empty;

            // Assert
            if (shouldInline)
            {
                expected = this.InlinedReferences;
            }
            else
            {
                expected = this.NoInlinedReferences;
            }

            expected = expected.MakeLineBreaksEnvironmentNeutral();

            Assert.AreEqual(expected, actual);
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
            var mainSchema = new AsyncApiSchema();
            var subSchema = new AsyncApiSchema();
            subSchema.Properties.Add("title", new AsyncApiSchema() { Type = SchemaType.String });
            mainSchema.OneOf = new List<AsyncApiSchema>() { subSchema };

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
            var mainSchema = new AsyncApiSchema();
            var subSchema = new AsyncApiSchema();
            subSchema.Properties.Add("title", new AsyncApiSchema() { Type = SchemaType.String });
            mainSchema.AnyOf = new List<AsyncApiSchema>() { subSchema };

            var yaml = mainSchema.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            Assert.True(!yaml.Contains("if:"));
        }

        /// <summary>
        /// Regression test.
        /// Bug: Serializing properties multiple times - specifically Schema.Not was serialized into Not and Else.
        /// </summary>
        [Test]
        public void Serialize_WithNot_DoesNotWriteElse()
        {
            var mainSchema = new AsyncApiSchema();
            var subSchema = new AsyncApiSchema();
            subSchema.Properties.Add("title", new AsyncApiSchema() { Type = SchemaType.String });
            mainSchema.Not = subSchema;

            var yaml = mainSchema.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            Assert.True(!yaml.Contains("else:"));
        }
    }
}
