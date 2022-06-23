﻿// Copyright (c) The LEGO Group. All rights reserved.

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Models
{
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
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Value MUST be a string. Multiple types via an array are not supported.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public decimal? Maximum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public bool? ExclusiveMaximum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public decimal? Minimum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public bool? ExclusiveMinimum { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
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
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> AllOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> OneOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> AnyOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public AsyncApiSchema Not { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public AsyncApiSchema Contains { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public AsyncApiSchema If { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public AsyncApiSchema Then { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public AsyncApiSchema Else { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public ISet<string> Required { get; set; } = new HashSet<string>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Value MUST be an object and not an array. Inline or referenced schema MUST be of a Schema Object
        /// and not a standard JSON Schema. items MUST be present if the type is array.
        /// </summary>
        public AsyncApiSchema Items { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MaxItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MinItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public bool? UniqueItems { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Property definitions MUST be a Schema Object and not a standard JSON Schema (inline or referenced).
        /// </summary>
        public IDictionary<string, AsyncApiSchema> Properties { get; set; } = new Dictionary<string, AsyncApiSchema>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MaxProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MinProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Value can be boolean or object. Inline or referenced schema
        /// MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public AsyncApiSchema AdditionalProperties { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public AsyncApiSchema PropertyNames { get; set; }

        /// <summary>
        /// adds support for polymorphism.
        /// The discriminator is the schema property name that is used to differentiate between other schema that inherit this schema.
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public IList<IAsyncApiAny> Enum { get; set; } = new List<IAsyncApiAny>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public IList<IAsyncApiAny> Examples { get; set; } = new List<IAsyncApiAny>();

        /// <summary>
        /// follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
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
            writer.WriteProperty(AsyncApiConstants.Title, Title);

            // multipleOf
            writer.WriteProperty(AsyncApiConstants.MultipleOf, MultipleOf);

            // maximum
            writer.WriteProperty(AsyncApiConstants.Maximum, Maximum);

            // exclusiveMaximum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMaximum, ExclusiveMaximum);

            // minimum
            writer.WriteProperty(AsyncApiConstants.Minimum, Minimum);

            // exclusiveMinimum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMinimum, ExclusiveMinimum);

            // maxLength
            writer.WriteProperty(AsyncApiConstants.MaxLength, MaxLength);

            // minLength
            writer.WriteProperty(AsyncApiConstants.MinLength, MinLength);

            // pattern
            writer.WriteProperty(AsyncApiConstants.Pattern, Pattern);

            // maxItems
            writer.WriteProperty(AsyncApiConstants.MaxItems, MaxItems);

            // minItems
            writer.WriteProperty(AsyncApiConstants.MinItems, MinItems);

            // uniqueItems
            writer.WriteProperty(AsyncApiConstants.UniqueItems, UniqueItems);

            // maxProperties
            writer.WriteProperty(AsyncApiConstants.MaxProperties, MaxProperties);

            // minProperties
            writer.WriteProperty(AsyncApiConstants.MinProperties, MinProperties);

            // required
            writer.WriteOptionalCollection(AsyncApiConstants.Required, Required, (w, s) => w.WriteValue(s));

            // enum
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, Enum, (nodeWriter, s) => nodeWriter.WriteAny(s));

            writer.WriteOptionalObject(AsyncApiConstants.Const, Const, (w, s) => w.WriteAny(s));

            // type
            writer.WriteProperty(AsyncApiConstants.Type, Type);

            // allOf
            writer.WriteOptionalCollection(AsyncApiConstants.AllOf, AllOf, (w, s) => s.SerializeV2(w));

            // anyOf
            writer.WriteOptionalCollection(AsyncApiConstants.AnyOf, AnyOf, (w, s) => s.SerializeV2(w));

            // oneOf
            writer.WriteOptionalCollection(AsyncApiConstants.OneOf, OneOf, (w, s) => s.SerializeV2(w));

            // not
            writer.WriteOptionalObject(AsyncApiConstants.Not, Not, (w, s) => s.SerializeV2(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Contains, AllOf, (w, s) => s.SerializeV2(w));

            // anyOf
            writer.WriteOptionalCollection(AsyncApiConstants.If, AnyOf, (w, s) => s.SerializeV2(w));

            // oneOf
            writer.WriteOptionalCollection(AsyncApiConstants.Then, OneOf, (w, s) => s.SerializeV2(w));

            // not
            writer.WriteOptionalObject(AsyncApiConstants.Else, Not, (w, s) => s.SerializeV2(w));

            // items
            writer.WriteOptionalObject(AsyncApiConstants.Items, Items, (w, s) => s.SerializeV2(w));

            // properties
            writer.WriteOptionalMap(AsyncApiConstants.Properties, Properties, (w, s) => s.SerializeV2(w));

            // additionalProperties
            writer.WriteOptionalObject(AsyncApiConstants.AdditionalProperties, AdditionalProperties, (w, s) => s.SerializeV2(w));

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // format
            writer.WriteProperty(AsyncApiConstants.Format, Format);

            // default
            writer.WriteOptionalObject(AsyncApiConstants.Default, Default, (w, d) => w.WriteAny(d));

            // nullable
            writer.WriteProperty(AsyncApiConstants.Nullable, Nullable, false);

            // discriminator
            writer.WriteProperty(AsyncApiConstants.Discriminator, Discriminator);

            // readOnly
            writer.WriteProperty(AsyncApiConstants.ReadOnly, ReadOnly, false);

            // writeOnly
            writer.WriteProperty(AsyncApiConstants.WriteOnly, WriteOnly, false);

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, s) => s.SerializeV2(w));

            // example
            writer.WriteOptionalCollection(AsyncApiConstants.Examples, Examples, (w, e) => w.WriteAny(e));

            // deprecated
            writer.WriteProperty(AsyncApiConstants.Deprecated, Deprecated, false);

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var settings = writer.GetSettings();

            if (Reference != null)
            {
                if (settings.ReferenceInline != ReferenceInlineSetting.InlineReferences)
                {
                    Reference.SerializeV2(writer);
                    return;
                }

                // If Loop is detected then just Serialize as a reference.
                if (!settings.LoopDetector.PushLoop(this))
                {
                    settings.LoopDetector.SaveLoop(this);
                    Reference.SerializeV2(writer);
                    return;
                }
            }

            SerializeV2WithoutReference(writer);

            if (Reference != null)
            {
                settings.LoopDetector.PopLoop<AsyncApiSchema>();
            }
        }
    }
}