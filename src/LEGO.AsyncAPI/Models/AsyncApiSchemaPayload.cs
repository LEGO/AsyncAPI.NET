// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiJsonSchemaPayload : IAsyncApiMessagePayload, IAsyncApiReferenceable
    {
        private readonly AsyncApiSchema schema;

        public AsyncApiJsonSchemaPayload()
        {
            this.schema = new AsyncApiSchema();
        }

        public AsyncApiJsonSchemaPayload(AsyncApiSchema schema)
        {
            this.schema = schema;
        }

        public string Title { get => this.schema.Title; set => this.schema.Title = value; }

        public SchemaType? Type { get => this.schema.Type; set => this.schema.Type = value; }

        public string Format { get => this.schema.Format; set => this.schema.Format = value; }

        public string Description { get => this.schema.Description; set => this.schema.Description = value; }

        public double? Maximum { get => this.schema.Maximum; set => this.schema.Maximum = value; }

        public bool? ExclusiveMaximum { get => this.schema.ExclusiveMaximum; set => this.schema.ExclusiveMaximum = value; }

        public double? Minimum { get => this.schema.Minimum; set => this.schema.Minimum = value; }

        public bool? ExclusiveMinimum { get => this.schema.ExclusiveMinimum; set => this.schema.ExclusiveMinimum = value; }

        public int? MaxLength { get => this.schema.MaxLength; set => this.schema.MaxLength = value; }

        public int? MinLength { get => this.schema.MinLength; set => this.schema.MinLength = value; }

        public string Pattern { get => this.schema.Pattern; set => this.schema.Pattern = value; }

        public double? MultipleOf { get => this.schema.MultipleOf; set => this.schema.MultipleOf = value; }

        public AsyncApiAny Default { get => this.schema.Default; set => this.schema.Default = value; }

        public bool ReadOnly { get => this.schema.ReadOnly; set => this.schema.ReadOnly = value; }

        public bool WriteOnly { get => this.schema.WriteOnly; set => this.schema.WriteOnly = value; }

        public IList<AsyncApiSchema> AllOf { get => this.schema.AllOf; set => this.schema.AllOf = value; }

        public IList<AsyncApiSchema> OneOf { get => this.schema.OneOf; set => this.schema.OneOf = value; }

        public IList<AsyncApiSchema> AnyOf { get => this.schema.AnyOf; set => this.schema.AnyOf = value; }

        public AsyncApiSchema Not { get => this.schema.Not; set => this.schema.Not = value; }

        public AsyncApiSchema Contains { get => this.schema.Contains; set => this.schema.Contains = value; }

        public AsyncApiSchema If { get => this.schema.If; set => this.schema.If = value; }

        public AsyncApiSchema Then { get => this.schema.Then; set => this.schema.Then = value; }

        public AsyncApiSchema Else { get => this.schema.Else; set => this.schema.Else = value; }

        public ISet<string> Required { get => this.schema.Required; set => this.schema.Required = value; }

        public AsyncApiSchema Items { get => this.schema.Items; set => this.schema.Items = value; }

        public AsyncApiSchema AdditionalItems { get => this.schema.AdditionalItems; set => this.schema.AdditionalItems = value; }

        public int? MaxItems { get => this.schema.MaxItems; set => this.schema.MaxItems = value; }

        public int? MinItems { get => this.schema.MinItems; set => this.schema.MinItems = value; }

        public bool? UniqueItems { get => this.schema.UniqueItems; set => this.schema.UniqueItems = value; }

        public IDictionary<string, AsyncApiSchema> Properties { get => this.schema.Properties; set => this.schema.Properties = value; }

        public int? MaxProperties { get => this.schema.MaxProperties; set => this.schema.MaxProperties = value; }

        public int? MinProperties { get => this.schema.MinProperties; set => this.schema.MinProperties = value; }

        public IDictionary<string, AsyncApiSchema> PatternProperties { get => this.schema.PatternProperties; set => this.schema.PatternProperties = value; }

        public AsyncApiSchema PropertyNames { get => this.schema.PropertyNames; set => this.schema.PropertyNames = value; }

        public string Discriminator { get => this.schema.Discriminator; set => this.schema.Discriminator = value; }

        public IList<AsyncApiAny> Enum { get => this.schema.Enum; set => this.schema.Enum = value; }

        public IList<AsyncApiAny> Examples { get => this.schema.Examples; set => this.schema.Examples = value; }

        public AsyncApiAny Const { get => this.schema.Const; set => this.schema.Const = value; }

        public bool Nullable { get => this.schema.Nullable; set => this.schema.Nullable = value; }

        public AsyncApiExternalDocumentation ExternalDocs { get => this.schema.ExternalDocs; set => this.schema.ExternalDocs = value; }

        public bool Deprecated { get => this.schema.Deprecated; set => this.schema.Deprecated = value; }

        public bool UnresolvedReference { get => this.schema.UnresolvedReference; set => this.schema.UnresolvedReference = value; }

        public AsyncApiReference Reference { get => this.schema.Reference; set => this.schema.Reference = value; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get => this.schema.Extensions; set => this.schema.Extensions = value; }

        public AsyncApiSchema AdditionalProperties { get => this.schema.AdditionalProperties; set => this.schema.AdditionalProperties = value; }

        public static implicit operator AsyncApiSchema(AsyncApiJsonSchemaPayload payload) => payload.schema;

        public static implicit operator AsyncApiJsonSchemaPayload(AsyncApiSchema schema) => new AsyncApiJsonSchemaPayload(schema);

        public void SerializeV2(IAsyncApiWriter writer)
        {
            this.schema.SerializeV2(writer);
        }

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            this.schema.SerializeV2WithoutReference(writer);
        }
    }
}
