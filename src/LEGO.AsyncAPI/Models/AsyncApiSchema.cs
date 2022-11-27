// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The Schema Object allows the definition of input and output data types.
    /// </summary>
    public class AsyncApiSchema : IAsyncApiReferenceable, IAsyncApiExtensible, IAsyncApiSerializable
    {
        /// <summary>
        /// follow JSON Schema definition. Short text providing information about the data.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// </summary>
        public IList<SchemaType> Type { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public decimal? Maximum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public bool? ExclusiveMaximum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public decimal? Minimum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public bool? ExclusiveMinimum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public decimal? MultipleOf { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// The default value represents what would be assumed by the consumer of the input as the value of the schema if one is not provided.
        /// Unlike JSON Schema, the value MUST conform to the defined type for the Schema Object defined at the same level.
        /// For example, if type is string, then default can be "foo" but cannot be 1.
        /// </summary>
        public IAsyncApiAny Default { get; set; }

        /// <summary>
        /// a value indicating whether relevant only for Schema "properties" definitions. Declares the property as "read only".
        /// This means that it MAY be sent as part of a response but SHOULD NOT be sent as part of the request.
        /// If the property is marked as readOnly being true and is in the required list,
        /// the required will take effect on the response only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// a value indicating whether relevant only for Schema "properties" definitions. Declares the property as "write only".
        /// Therefore, it MAY be sent as part of a request but SHOULD NOT be sent as part of the response.
        /// If the property is marked as writeOnly being true and is in the required list,
        /// the required will take effect on the request only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        public bool WriteOnly { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> AllOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> OneOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> AnyOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public AsyncApiSchema Not { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public AsyncApiSchema Contains { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public AsyncApiSchema If { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public AsyncApiSchema Then { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public AsyncApiSchema Else { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public ISet<string> Required { get; set; } = new HashSet<string>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Value MUST be an object and not an array. Inline or referenced schema MUST be of a Schema Object
        /// and not a standard JSON Schema. items MUST be present if the type is array.
        /// </summary>
        public AsyncApiSchema Items { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public int? MaxItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public int? MinItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public bool? UniqueItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Property definitions MUST be a Schema Object and not a standard JSON Schema (inline or referenced).
        /// </summary>
        public IDictionary<string, AsyncApiSchema> Properties { get; set; } = new Dictionary<string, AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public int? MaxProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public int? MinProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Value can be boolean or object. Inline or referenced schema
        /// MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public AsyncApiSchema AdditionalProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public AsyncApiSchema PropertyNames { get; set; }

        /// <summary>
        /// adds support for polymorphism.
        /// The discriminator is the schema property name that is used to differentiate between other schema that inherit this schema.
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public IList<IAsyncApiAny> Enum { get; set; } = new List<IAsyncApiAny>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public IList<IAsyncApiAny> Examples { get; set; } = new List<IAsyncApiAny>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public IAsyncApiAny Const { get; set; }

        /// <summary>
        /// a value indicating whether allows sending a null value for the defined schema. Default value is false.
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// additional external documentation for this schema.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// a value indicating whether specifies that a schema is deprecated and SHOULD be transitioned out of usage.
        /// Default value is false.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <inheritdoc/>
        public bool UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // title
            writer.WriteOptionalProperty(AsyncApiConstants.Title, this.Title);

            // type
            if (this.Type != null)
            {
                if (this.Type.Count == 1)
                {
                    writer.WriteOptionalProperty(AsyncApiConstants.Type, this.Type.First().GetDisplayName());
                }
                else
                {
                    writer.WriteOptionalCollection(AsyncApiConstants.Type, this.Type.Select(t => t.GetDisplayName()), (w, s) => w.WriteValue(s));
                }
            }

            // format
            writer.WriteOptionalProperty(AsyncApiConstants.Format, this.Format);

            // description
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            // maximum
            writer.WriteOptionalProperty(AsyncApiConstants.Maximum, this.Maximum);

            // exclusiveMaximum
            writer.WriteOptionalProperty(AsyncApiConstants.ExclusiveMaximum, this.ExclusiveMaximum);

            // minimum
            writer.WriteOptionalProperty(AsyncApiConstants.Minimum, this.Minimum);

            // exclusiveMinimum
            writer.WriteOptionalProperty(AsyncApiConstants.ExclusiveMinimum, this.ExclusiveMinimum);

            // maxLength
            writer.WriteOptionalProperty(AsyncApiConstants.MaxLength, this.MaxLength);

            // minLength
            writer.WriteOptionalProperty(AsyncApiConstants.MinLength, this.MinLength);

            // pattern
            writer.WriteOptionalProperty(AsyncApiConstants.Pattern, this.Pattern);

            // multipleOf
            writer.WriteOptionalProperty(AsyncApiConstants.MultipleOf, this.MultipleOf);

            // default
            writer.WriteOptionalObject(AsyncApiConstants.Default, this.Default, (w, d) => w.WriteAny(d));

            // readOnly
            writer.WriteOptionalProperty(AsyncApiConstants.ReadOnly, this.ReadOnly, false);

            // writeOnly
            writer.WriteOptionalProperty(AsyncApiConstants.WriteOnly, this.WriteOnly, false);

            // allOf
            writer.WriteOptionalCollection(AsyncApiConstants.AllOf, this.AllOf, (w, s) => s.SerializeV2(w));

            // oneOf
            writer.WriteOptionalCollection(AsyncApiConstants.OneOf, this.OneOf, (w, s) => s.SerializeV2(w));

            // anyOf
            writer.WriteOptionalCollection(AsyncApiConstants.AnyOf, this.AnyOf, (w, s) => s.SerializeV2(w));

            // not
            writer.WriteOptionalObject(AsyncApiConstants.Not, this.Not, (w, s) => s.SerializeV2(w));

            // contains
            writer.WriteOptionalObject(AsyncApiConstants.Contains, this.Contains, (w, s) => s.SerializeV2(w));

            // anyOf
            writer.WriteOptionalObject(AsyncApiConstants.If, this.If, (w, s) => s.SerializeV2(w));

            // then
            writer.WriteOptionalObject(AsyncApiConstants.Then, this.Then, (w, s) => s.SerializeV2(w));

            // else
            writer.WriteOptionalObject(AsyncApiConstants.Else, this.Else, (w, s) => s.SerializeV2(w));

            // required
            writer.WriteOptionalCollection(AsyncApiConstants.Required, this.Required, (w, s) => w.WriteValue(s));

            // items
            writer.WriteOptionalObject(AsyncApiConstants.Items, this.Items, (w, s) => s.SerializeV2(w));

            // maxItems
            writer.WriteOptionalProperty(AsyncApiConstants.MaxItems, this.MaxItems);

            // minItems
            writer.WriteOptionalProperty(AsyncApiConstants.MinItems, this.MinItems);

            // uniqueItems
            writer.WriteOptionalProperty(AsyncApiConstants.UniqueItems, this.UniqueItems);

            // properties
            writer.WriteOptionalMap(AsyncApiConstants.Properties, this.Properties, (w, s) => s.SerializeV2(w));

            // maxProperties
            writer.WriteOptionalProperty(AsyncApiConstants.MaxProperties, this.MaxProperties);

            // minProperties
            writer.WriteOptionalProperty(AsyncApiConstants.MinProperties, this.MinProperties);

            // additionalProperties
            writer.WriteOptionalObject(AsyncApiConstants.AdditionalProperties, this.AdditionalProperties, (w, s) => s.SerializeV2(w));

            // discriminator
            writer.WriteOptionalProperty(AsyncApiConstants.Discriminator, this.Discriminator);

            // enum
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, this.Enum, (nodeWriter, s) => nodeWriter.WriteAny(s));

            // example
            writer.WriteOptionalCollection(AsyncApiConstants.Examples, this.Examples, (w, e) => w.WriteAny(e));

            writer.WriteOptionalObject(AsyncApiConstants.Const, this.Const, (w, s) => w.WriteAny(s));

            // nullable
            writer.WriteOptionalProperty(AsyncApiConstants.Nullable, this.Nullable, false);

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, s) => s.SerializeV2(w));

            // deprecated
            writer.WriteOptionalProperty(AsyncApiConstants.Deprecated, this.Deprecated, false);

            // extensions
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var settings = writer.GetSettings();

            if (this.Reference != null)
            {
                if (settings.ReferenceInline != ReferenceInlineSetting.InlineReferences)
                {
                    this.Reference.SerializeV2(writer);
                    return;
                }

                // If Loop is detected then just Serialize as a reference.
                if (!settings.LoopDetector.PushLoop(this))
                {
                    settings.LoopDetector.SaveLoop(this);
                    this.Reference.SerializeV2(writer);
                    return;
                }
            }

            this.SerializeV2WithoutReference(writer);

            if (this.Reference != null)
            {
                settings.LoopDetector.PopLoop<AsyncApiSchema>();
            }
        }
    }
}