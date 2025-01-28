// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models{    using System.Collections.Generic;    using LEGO.AsyncAPI.Models.Interfaces;    using LEGO.AsyncAPI.Writers;    public class AsyncApiJsonSchemaPayload : IAsyncApiMessagePayload    {        private readonly AsyncApiJsonSchema schema;

        public AsyncApiJsonSchemaPayload()
        {
            this.schema = new AsyncApiJsonSchema();
        }

        public AsyncApiJsonSchemaPayload(AsyncApiJsonSchema schema)
        {
            this.schema = schema;
        }

        public virtual string Title { get => this.schema.Title; set => this.schema.Title = value; }

        public virtual SchemaType? Type { get => this.schema.Type; set => this.schema.Type = value; }

        public virtual string Format { get => this.schema.Format; set => this.schema.Format = value; }

        public virtual string Description { get => this.schema.Description; set => this.schema.Description = value; }

        public virtual double? Maximum { get => this.schema.Maximum; set => this.schema.Maximum = value; }

        public virtual double? ExclusiveMaximum { get => this.schema.ExclusiveMaximum; set => this.schema.ExclusiveMaximum = value; }

        public virtual double? Minimum { get => this.schema.Minimum; set => this.schema.Minimum = value; }

        public virtual double? ExclusiveMinimum { get => this.schema.ExclusiveMinimum; set => this.schema.ExclusiveMinimum = value; }

        public virtual int? MaxLength { get => this.schema.MaxLength; set => this.schema.MaxLength = value; }

        public virtual int? MinLength { get => this.schema.MinLength; set => this.schema.MinLength = value; }

        public virtual string Pattern { get => this.schema.Pattern; set => this.schema.Pattern = value; }

        public virtual double? MultipleOf { get => this.schema.MultipleOf; set => this.schema.MultipleOf = value; }

        public virtual AsyncApiAny Default { get => this.schema.Default; set => this.schema.Default = value; }

        public virtual bool ReadOnly { get => this.schema.ReadOnly; set => this.schema.ReadOnly = value; }

        public virtual bool WriteOnly { get => this.schema.WriteOnly; set => this.schema.WriteOnly = value; }

        public virtual IList<AsyncApiJsonSchema> AllOf { get => this.schema.AllOf; set => this.schema.AllOf = value; }

        public virtual IList<AsyncApiJsonSchema> OneOf { get => this.schema.OneOf; set => this.schema.OneOf = value; }

        public virtual IList<AsyncApiJsonSchema> AnyOf { get => this.schema.AnyOf; set => this.schema.AnyOf = value; }

        public virtual AsyncApiJsonSchema Not { get => this.schema.Not; set => this.schema.Not = value; }

        public virtual AsyncApiJsonSchema Contains { get => this.schema.Contains; set => this.schema.Contains = value; }

        public virtual AsyncApiJsonSchema If { get => this.schema.If; set => this.schema.If = value; }

        public virtual AsyncApiJsonSchema Then { get => this.schema.Then; set => this.schema.Then = value; }

        public virtual AsyncApiJsonSchema Else { get => this.schema.Else; set => this.schema.Else = value; }

        public virtual ISet<string> Required { get => this.schema.Required; set => this.schema.Required = value; }

        public virtual AsyncApiJsonSchema Items { get => this.schema.Items; set => this.schema.Items = value; }

        public virtual AsyncApiJsonSchema AdditionalItems { get => this.schema.AdditionalItems; set => this.schema.AdditionalItems = value; }

        public virtual int? MaxItems { get => this.schema.MaxItems; set => this.schema.MaxItems = value; }

        public virtual int? MinItems { get => this.schema.MinItems; set => this.schema.MinItems = value; }

        public virtual bool? UniqueItems { get => this.schema.UniqueItems; set => this.schema.UniqueItems = value; }

        public virtual IDictionary<string, AsyncApiJsonSchema> Properties { get => this.schema.Properties; set => this.schema.Properties = value; }

        public virtual int? MaxProperties { get => this.schema.MaxProperties; set => this.schema.MaxProperties = value; }

        public virtual int? MinProperties { get => this.schema.MinProperties; set => this.schema.MinProperties = value; }

        public virtual IDictionary<string, AsyncApiJsonSchema> PatternProperties { get => this.schema.PatternProperties; set => this.schema.PatternProperties = value; }

        public virtual AsyncApiJsonSchema PropertyNames { get => this.schema.PropertyNames; set => this.schema.PropertyNames = value; }

        public virtual string Discriminator { get => this.schema.Discriminator; set => this.schema.Discriminator = value; }

        public virtual IList<AsyncApiAny> Enum { get => this.schema.Enum; set => this.schema.Enum = value; }

        public virtual IList<AsyncApiAny> Examples { get => this.schema.Examples; set => this.schema.Examples = value; }

        public virtual AsyncApiAny Const { get => this.schema.Const; set => this.schema.Const = value; }

        public virtual bool Nullable { get => this.schema.Nullable; set => this.schema.Nullable = value; }

        public virtual AsyncApiExternalDocumentation ExternalDocs { get => this.schema.ExternalDocs; set => this.schema.ExternalDocs = value; }

        public virtual bool Deprecated { get => this.schema.Deprecated; set => this.schema.Deprecated = value; }

        public virtual IDictionary<string, IAsyncApiExtension> Extensions { get => this.schema.Extensions; set => this.schema.Extensions = value; }

        public virtual AsyncApiJsonSchema AdditionalProperties { get => this.schema.AdditionalProperties; set => this.schema.AdditionalProperties = value; }

        public static implicit operator AsyncApiJsonSchema(AsyncApiJsonSchemaPayload payload) => payload.schema;

        public static implicit operator AsyncApiJsonSchemaPayload(AsyncApiJsonSchema schema) => new AsyncApiJsonSchemaPayload(schema);

        public virtual void SerializeV2(IAsyncApiWriter writer)
        {
            this.schema.SerializeV2(writer);
        }
    }}