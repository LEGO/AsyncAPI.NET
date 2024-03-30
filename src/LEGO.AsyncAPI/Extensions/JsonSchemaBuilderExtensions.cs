// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using Json.Schema;
    using LEGO.AsyncAPI.Models.Interfaces;

    public static class JsonSchemaBuilderExtensions
    {
        /// <summary>
        /// Custom extensions in the schema
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public static JsonSchemaBuilder Extensions(this JsonSchemaBuilder builder, IDictionary<string, IAsyncApiExtension> extensions)
        {
            builder.Add(new ExtensionsKeyword(extensions));
            return builder;
        }

        /// <summary>
        /// Allows sending a null value for the defined schema. Default value is false.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JsonSchemaBuilder Nullable(this JsonSchemaBuilder builder, bool value)
        {
            builder.Add(new NullableKeyword(value));
            return builder;
        }

        /// <summary>
        /// Additional external documentation for this schema.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JsonSchemaBuilder ExternalDocs(this JsonSchemaBuilder builder, AsyncApiExternalDocumentation value)
        {
            builder.Add(new ExternalDocsKeyword(value));
            return builder;
        }

        /// <summary>
        /// Adds support for polymorphism. The discriminator is an object name that is used to differentiate
        /// between other schemas which may satisfy the payload description.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="discriminator"></param>
        /// <returns></returns>
        public static JsonSchemaBuilder Discriminator(this JsonSchemaBuilder builder, string discriminator)
        {
            builder.Add(new DiscriminatorKeyword(discriminator));
            return builder;
        }
    }

    /// <summary>
    /// The nullable keyword
    /// </summary>
    [SchemaKeyword(Name)]
    public class NullableKeyword : IJsonSchemaKeyword
    {
        public const string Name = "nullable";

        public bool Value { get; }

        public NullableKeyword(bool value)
        {
            Value = value;
        }

        public void Evaluate(EvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public KeywordConstraint GetConstraint(SchemaConstraint schemaConstraint, IReadOnlyList<KeywordConstraint> localConstraints, EvaluationContext context)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The extensions keyword
    /// </summary>
    [SchemaKeyword(Name)]
    public class ExternalDocsKeyword : IJsonSchemaKeyword
    {
        /// <summary>
        /// The schema keyword name
        /// </summary>
        public const string Name = "externalDocs";

        internal AsyncApiExternalDocumentation ExternalDocs { get; }

        internal ExternalDocsKeyword(AsyncApiExternalDocumentation externalDocs)
        {
            ExternalDocs = externalDocs;
        }

        public void Evaluate(EvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public KeywordConstraint GetConstraint(SchemaConstraint schemaConstraint, IReadOnlyList<KeywordConstraint> localConstraints, EvaluationContext context)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The extensions keyword.
    /// </summary>
    [SchemaKeyword(Name)]
    public class ExtensionsKeyword : IJsonSchemaKeyword
    {
        /// <summary>
        /// The schema keyword name
        /// </summary>
        public const string Name = "extensions";

        internal IDictionary<string, IAsyncApiExtension> Extensions { get; }

        internal ExtensionsKeyword(IDictionary<string, IAsyncApiExtension> extensions)
        {
            Extensions = extensions;
        }

        public void Evaluate(EvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public KeywordConstraint GetConstraint(SchemaConstraint schemaConstraint, IReadOnlyList<KeywordConstraint> localConstraints, EvaluationContext context)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The AdditionalPropertiesAllowed Keyword.
    /// </summary>
    [SchemaKeyword(Name)]
    public class AdditionalPropertiesAllowedKeyword : IJsonSchemaKeyword
    {
        /// <summary>
        /// The schema keyword name
        /// </summary>
        public const string Name = "additionalPropertiesAllowed";

        internal bool AdditionalPropertiesAllowed { get; }

        internal AdditionalPropertiesAllowedKeyword(bool additionalPropertiesAllowed)
        {
            AdditionalPropertiesAllowed = additionalPropertiesAllowed;
        }

        public void Evaluate(EvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public KeywordConstraint GetConstraint(SchemaConstraint schemaConstraint, IReadOnlyList<KeywordConstraint> localConstraints, EvaluationContext context)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The Discriminator Keyword
    /// </summary>
    [SchemaKeyword(Name)]
    public class DiscriminatorKeyword : IJsonSchemaKeyword
    {
        /// <summary>
        /// The schema keyword name
        /// </summary>
        public const string Name = "discriminator";

        internal string Descriminator { get; }

        public DiscriminatorKeyword(string descriminator)
        {
            this.Descriminator = descriminator;
        }

        public void Evaluate(EvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public KeywordConstraint GetConstraint(SchemaConstraint schemaConstraint, IReadOnlyList<KeywordConstraint> localConstraints, EvaluationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
