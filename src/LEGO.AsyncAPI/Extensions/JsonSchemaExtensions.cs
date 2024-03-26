// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Json.Schema;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Writers;
    using System.Text.RegularExpressions;

    public static class JsonSchemaExtensions
    {
        public static void SerializeV2WithoutReference(this JsonSchema schema, IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // title
            writer.WriteOptionalProperty(AsyncApiConstants.Title, schema.GetTitle());

            // type
            if (schema.GetJsonType() != null)
            {
                var types = EnumExtensions.GetFlags<SchemaValueType>(schema.GetJsonType());
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
            writer.WriteOptionalProperty(AsyncApiConstants.Format, schema.GetFormat()?.Key);

            // description
            writer.WriteOptionalProperty(AsyncApiConstants.Description, schema.GetDescription());

            // maximum
            writer.WriteOptionalProperty(AsyncApiConstants.Maximum, schema.GetMaximum());

            // exclusiveMaximum
            writer.WriteOptionalProperty(AsyncApiConstants.ExclusiveMaximum, schema.GetExclusiveMaximum());

            // minimum
            writer.WriteOptionalProperty(AsyncApiConstants.Minimum, schema.GetMinimum());

            // exclusiveMinimum
            writer.WriteOptionalProperty(AsyncApiConstants.ExclusiveMinimum, schema.GetExclusiveMinimum());

            // maxLength
            writer.WriteOptionalProperty(AsyncApiConstants.MaxLength, schema.GetMaxLength());

            // minLength
            writer.WriteOptionalProperty(AsyncApiConstants.MinLength, schema.GetMinLength());

            // pattern
            writer.WriteOptionalProperty(AsyncApiConstants.Pattern, schema.GetPattern().ToString());

            // multipleOf
            writer.WriteOptionalProperty(AsyncApiConstants.MultipleOf, schema.GetMultipleOf());

            // default
            writer.WriteOptionalObject(AsyncApiConstants.Default, schema.GetDefault(), (w, d) => w.WriteAny(d));

            // readOnly
            writer.WriteOptionalProperty(AsyncApiConstants.ReadOnly, schema.GetReadOnly(), false);

            // writeOnly
            writer.WriteOptionalProperty(AsyncApiConstants.WriteOnly, schema.GetWriteOnly(), false);

            // allOf
            writer.WriteOptionalCollection(AsyncApiConstants.AllOf, schema.GetAllOf(), (w, s) => s.SerializeV2(w));

            // oneOf
            writer.WriteOptionalCollection(AsyncApiConstants.OneOf, schema.GetOneOf(), (w, s) => s.SerializeV2(w));

            // anyOf
            writer.WriteOptionalCollection(AsyncApiConstants.AnyOf, schema.GetAnyOf(), (w, s) => s.SerializeV2(w));

            // not
            writer.WriteOptionalObject(AsyncApiConstants.Not, schema.GetNot(), (w, s) => s.SerializeV2(w));

            // contains
            writer.WriteOptionalObject(AsyncApiConstants.Contains, schema.GetContains(), (w, s) => s.SerializeV2(w));

            // anyOf
            writer.WriteOptionalObject(AsyncApiConstants.If, schema.GetIf(), (w, s) => s.SerializeV2(w));

            // then
            writer.WriteOptionalObject(AsyncApiConstants.Then, schema.GetThen(), (w, s) => s.SerializeV2(w));

            // else
            writer.WriteOptionalObject(AsyncApiConstants.Else, schema.GetElse(), (w, s) => s.SerializeV2(w));

            // required
            writer.WriteOptionalCollection(AsyncApiConstants.Required, schema.GetRequired(), (w, s) => w.WriteValue(s));

            // items
            writer.WriteOptionalObject(AsyncApiConstants.Items, schema.GetItems(), (w, s) => s.SerializeV2(w));

            // additionalItems
            writer.WriteOptionalObject(AsyncApiConstants.AdditionalItems, schema.GetAdditionalItems(), (w, s) => s.SerializeV2(w));

            // maxItems
            writer.WriteOptionalProperty(AsyncApiConstants.MaxItems, schema.GetMaxItems());

            // minItems
            writer.WriteOptionalProperty(AsyncApiConstants.MinItems, schema.GetMinItems());

            // uniqueItems
            writer.WriteOptionalProperty(AsyncApiConstants.UniqueItems, schema.GetUniqueItems());

            // properties
            writer.WriteOptionalMap(AsyncApiConstants.Properties, (IDictionary<string, JsonSchema>)schema.GetProperties(), (w, key, s) => s.SerializeV2(w));

            // maxProperties
            writer.WriteOptionalProperty(AsyncApiConstants.MaxProperties, schema.GetMaxProperties());

            // minProperties
            writer.WriteOptionalProperty(AsyncApiConstants.MinProperties, schema.GetMinProperties());

            // additionalProperties
            writer.WriteOptionalObject(AsyncApiConstants.AdditionalProperties, schema.GetAdditionalProperties(), (w, s) => s.SerializeV2(w));

            writer.WriteOptionalMap(AsyncApiConstants.PatternProperties, schema.GetPatternProperties().ToDictionary(d => d.Key.ToString(), d => d.Value), (w, key, s) => s.SerializeV2(w));

            writer.WriteOptionalObject(AsyncApiConstants.PropertyNames, schema.GetPropertyNames(), (w, s) => s.SerializeV2(w));

            // discriminator
            writer.WriteOptionalProperty(AsyncApiConstants.Discriminator, schema.GetDiscriminator());

            // enum
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, schema.GetEnum(), (w, s) => w.WriteAny(s));

            // example
            writer.WriteOptionalCollection(AsyncApiConstants.Examples, schema.GetExamples(), (w, e) => w.WriteAny(e));

            writer.WriteOptionalObject(AsyncApiConstants.Const, schema.GetConst(), (w, s) => w.WriteAny(s));

            // nullable
            writer.WriteOptionalProperty(AsyncApiConstants.Nullable, schema.GetNullable(), false);

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, schema.GetExternalDocs(), (w, s) => s.SerializeV2(w));

            // deprecated
            writer.WriteOptionalProperty(AsyncApiConstants.Deprecated, schema.GetDeprecated(), false);

            // extensions
            writer.WriteExtensions(schema.GetExtensions());

            writer.WriteEndObject();
        }

        public static void WriteJsonSchemaReference(this IAsyncApiWriter writer, Uri reference)
        {
            writer.WriteStartObject();
            writer.WriteRequiredProperty(AsyncApiConstants.DollarRef, reference.OriginalString);
            writer.WriteEndObject();
        }

        public static void SerializeV2(this JsonSchema schema, IAsyncApiWriter writer)
        {
            if (schema == null)
            {
                return;
            }

            var reference = schema.GetRef();
            var settings = writer.GetSettings();
            if (reference != null)
            {
                if (!settings.InlineReferences)
                {
                    writer.WriteJsonSchemaReference(reference);
                    return;
                }
                else
                {
                    if (!settings.LoopDetector.PushLoop(schema))
                    {
                        settings.LoopDetector.SaveLoop(schema);
                        writer.WriteJsonSchemaReference(reference);
                        return;
                    }
                }
            }

            schema.SerializeV2WithoutReference(writer);

            if (reference != null)
            {
                settings.LoopDetector.PopLoop<JsonSchema>();
            }
        }

        /// <summary>
        /// Gets the `discriminator` keyword if it exists.
        /// </summary>
        public static string GetDiscriminator(this JsonSchema schema)
        {
            return schema.TryGetKeyword<DiscriminatorKeyword>(DiscriminatorKeyword.Name, out var k) ? k.Descriminator! : null;
        }

        /// <summary>
        /// Gets the `ExternalDocs` keyword if it exists.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public static AsyncApiExternalDocumentation GetExternalDocs(this JsonSchema schema)
        {
            return schema.TryGetKeyword<ExternalDocsKeyword>(ExternalDocsKeyword.Name, out var k) ? k.ExternalDocs! : null;
        }

        /// <summary>
        /// Gets the nullable value if it exists
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool? GetNullable(this JsonSchema schema)
        {
            return schema.TryGetKeyword<NullableKeyword>(NullableKeyword.Name, out var k) ? k.Value! : null;
        }

        /// <summary>
        /// Gets the additional properties value if it exists
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool? GetAdditionalPropertiesAllowed(this JsonSchema schema)
        {
            return schema.TryGetKeyword<AdditionalPropertiesAllowedKeyword>(AdditionalPropertiesAllowedKeyword.Name, out var k) ? k.AdditionalPropertiesAllowed! : null;
        }

        /// <summary>
        /// Gets the custom extensions if it exists
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static IDictionary<string, IAsyncApiExtension> GetExtensions(this JsonSchema schema)
        {
            return schema.TryGetKeyword<ExtensionsKeyword>(ExtensionsKeyword.Name, out var k) ? k.Extensions! : null;
        }
    }
}
