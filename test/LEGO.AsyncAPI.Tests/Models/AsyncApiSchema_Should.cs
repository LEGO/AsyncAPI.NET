using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Writers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiSchema_Should
    {
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
                                Type = new List<SchemaType> { SchemaType.Object },
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
            .WithComponent("testD", new AsyncApiSchema() { Type = new List<SchemaType> { SchemaType.String }, Format = "uuid" })
            .WithComponent("testC", new AsyncApiSchema()
            {
                Type = new List<SchemaType> { SchemaType.Object },
                Properties = new Dictionary<string, AsyncApiSchema>
                {
                    { "testD", new AsyncApiSchema { Reference = new AsyncApiReference { Type = ReferenceType.Schema, Id = "testD" } } },
                },
            })
            .WithComponent("testB", new AsyncApiSchema() { Description = "test", Type = new List<SchemaType> { SchemaType.Boolean } })
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

            Assert.AreEqual(actual, expected);
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
            subSchema.Properties.Add("title", new AsyncApiSchema() { Type = new List<SchemaType> { SchemaType.String } });
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
            subSchema.Properties.Add("title", new AsyncApiSchema() { Type = new List<SchemaType> { SchemaType.String } });
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
            subSchema.Properties.Add("title", new AsyncApiSchema() { Type = new List<SchemaType> { SchemaType.String } });
            mainSchema.Not = subSchema;

            var yaml = mainSchema.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            Assert.True(!yaml.Contains("else:"));
        }
    }
}
