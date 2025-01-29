﻿// Copyright (c) The LEGO Group. All rights reserved.

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
    public class AsyncApiJsonSchema : IAsyncApiExtensible, IAsyncApiSerializable, IAsyncApiMessagePayload
    {
        /// <summary>
        /// follow JSON Schema definition. Short text providing information about the data.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual SchemaType? Type { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual string Format { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual double? Maximum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual double? ExclusiveMaximum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual double? Minimum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual double? ExclusiveMinimum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual int? MaxLength { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual int? MinLength { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect.
        /// </summary>
        public virtual string Pattern { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual double? MultipleOf { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// The default value represents what would be assumed by the consumer of the input as the value of the schema if one is not provided.
        /// Unlike JSON Schema, the value MUST conform to the defined type for the Schema Object defined at the same level.
        /// For example, if type is string, then default can be "foo" but cannot be 1.
        /// </summary>
        public virtual AsyncApiAny Default { get; set; }

        /// <summary>
        /// a value indicating whether relevant only for Schema "properties" definitions. Declares the property as "read only".
        /// This means that it MAY be sent as part of a response but SHOULD NOT be sent as part of the request.
        /// If the property is marked as readOnly being true and is in the required list,
        /// the required will take effect on the response only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        public virtual bool ReadOnly { get; set; }

        /// <summary>
        /// a value indicating whether relevant only for Schema "properties" definitions. Declares the property as "write only".
        /// Therefore, it MAY be sent as part of a request but SHOULD NOT be sent as part of the response.
        /// If the property is marked as writeOnly being true and is in the required list,
        /// the required will take effect on the request only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        public virtual bool WriteOnly { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public virtual IList<AsyncApiJsonSchema> AllOf { get; set; } = new List<AsyncApiJsonSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public virtual IList<AsyncApiJsonSchema> OneOf { get; set; } = new List<AsyncApiJsonSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public virtual IList<AsyncApiJsonSchema> AnyOf { get; set; } = new List<AsyncApiJsonSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public virtual AsyncApiJsonSchema Not { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual AsyncApiJsonSchema Contains { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual AsyncApiJsonSchema If { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual AsyncApiJsonSchema Then { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual AsyncApiJsonSchema Else { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual ISet<string> Required { get; set; } = new HashSet<string>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Value MUST be an object and not an array. Inline or referenced schema MUST be of a Schema Object
        /// and not a standard JSON Schema. items MUST be present if the type is array.
        /// </summary>
        public virtual AsyncApiJsonSchema Items { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Value MUST be an object and not an array. Inline or referenced schema MUST be of a Schema Object
        /// and not a standard JSON Schema. items MUST be present if the type is array.
        /// </summary>
        public virtual AsyncApiJsonSchema AdditionalItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual int? MaxItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual int? MinItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual bool? UniqueItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Property definitions MUST be a Schema Object and not a standard JSON Schema (inline or referenced).
        /// </summary>
        public virtual IDictionary<string, AsyncApiJsonSchema> Properties { get; set; } = new Dictionary<string, AsyncApiJsonSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual int? MaxProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual int? MinProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html
        /// Value can be boolean or object. Inline or referenced schema
        /// MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public virtual AsyncApiJsonSchema AdditionalProperties { get; set; }

        public virtual IDictionary<string, AsyncApiJsonSchema> PatternProperties { get; set; } = new Dictionary<string, AsyncApiJsonSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual AsyncApiJsonSchema PropertyNames { get; set; }

        /// <summary>
        /// adds support for polymorphism.
        /// The discriminator is the schema property name that is used to differentiate between other schema that inherit this schema.
        /// </summary>
        public virtual string Discriminator { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual IList<AsyncApiAny> Enum { get; set; } = new List<AsyncApiAny>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual IList<AsyncApiAny> Examples { get; set; } = new List<AsyncApiAny>();

        /// <summary>
        /// follow JSON Schema definition: https://json-schema.org/draft-07/json-schema-release-notes.html.
        /// </summary>
        public virtual AsyncApiAny Const { get; set; }

        /// <summary>
        /// a value indicating whether allows sending a null value for the defined schema. Default value is false.
        /// </summary>
        public virtual bool Nullable { get; set; }

        /// <summary>
        /// additional external documentation for this schema.
        /// </summary>
        public virtual AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// a value indicating whether specifies that a schema is deprecated and SHOULD be transitioned out of usage.
        /// Default value is false.
        /// </summary>
        public virtual bool Deprecated { get; set; }

        public virtual IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public virtual void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // title
            writer.WriteOptionalProperty(AsyncApiConstants.Title, this.Title);

            // type
            if (this.Type != null)
            {
                var types = EnumExtensions.GetFlags<SchemaType>(this.Type.Value);
                if (types.Count() == 1)
                {
                    writer.WriteOptionalProperty(AsyncApiConstants.Type, types.First().GetDisplayName());
                }
                else
                {
                    writer.WriteOptionalCollection(AsyncApiConstants.Type, types.Select(t => t.GetDisplayName()), (w, s) => w.WriteValue(s));
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
            if (this.Items is FalseApiSchema)
            {
                writer.WriteOptionalProperty<bool>(AsyncApiConstants.Items, false);
            }
            else
            {
                writer.WriteOptionalObject(AsyncApiConstants.Items, this.Items, (w, s) => s.SerializeV2(w));
            }

            // additionalItems
            if (this.AdditionalItems is FalseApiSchema)
            {
                writer.WriteOptionalProperty<bool>(AsyncApiConstants.AdditionalItems, false);
            }
            else
            {
                writer.WriteOptionalObject(AsyncApiConstants.AdditionalItems, this.AdditionalItems, (w, s) => s.SerializeV2(w));
            }

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
            if (this.AdditionalProperties is FalseApiSchema)
            {
                writer.WriteOptionalProperty<bool>(AsyncApiConstants.AdditionalProperties, false);
            }
            else
            {
                writer.WriteOptionalObject(AsyncApiConstants.AdditionalProperties, this.AdditionalProperties, (w, s) => s.SerializeV2(w));
            }

            writer.WriteOptionalMap(AsyncApiConstants.PatternProperties, this.PatternProperties, (w, s) => s.SerializeV2(w));

            writer.WriteOptionalObject(AsyncApiConstants.PropertyNames, this.PropertyNames, (w, s) => s.SerializeV2(w));

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
    }
}