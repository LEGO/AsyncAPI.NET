using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiSchema_Should
    {
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
