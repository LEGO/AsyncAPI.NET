// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Json.Schema;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public class JsonSchemaDeserializer
    {
        private static readonly FixedFieldMap<JsonSchemaBuilder> schemaFixedFields = new()
        {
            {
                "title", (a, n) => { a.Title(n.GetScalarValue()); }
            },
            {
                "type", (a, n) =>
                {
                   if(n is ListNode)
                    {
                        a.Type(n.CreateSimpleList(s => SchemaTypeConverter.ConvertToSchemaValueType(s.GetScalarValue())));
                    }
                    else
                    {
                        a.Type(SchemaTypeConverter.ConvertToSchemaValueType(n.GetScalarValue()));
                    }
                }
            },
            {
                "required",
                (a, n) => { a.Required(new HashSet<string>(n.CreateSimpleList(n2 => n2.GetScalarValue()))); }
            },
            {
                "multipleOf",
                (a, n) =>
                {
                    a.MultipleOf(decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture));
                }
            },
            {
                "maximum",
                (a, n) =>
                {
                    a.Maximum(decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture));
                }
            },
            {
                "exclusiveMaximum", (a, n) => { a.ExclusiveMaximum(decimal.Parse(n.GetScalarValue())); }
            },
            {
                "minimum",
                (a, n) =>
                {
                    a.Minimum(decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture));
                }
            },
            {
                "exclusiveMinimum", (a, n) => { a.ExclusiveMinimum(decimal.Parse(n.GetScalarValue())); }
            },
            {
                "maxLength", (a, n) => { a.MaxLength(uint.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture)); }
            },
            {
                "minLength", (a, n) => { a.MinLength(uint.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture)); }
            },
            {
                "pattern", (a, n) => { a.Pattern(n.GetScalarValue()); }
            },
            {
                "maxItems", (a, n) => { a.MaxItems(uint.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture)); }
            },
            {
                "minItems", (a, n) => { a.MinItems(uint.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture)); }
            },
            {
                "uniqueItems", (a, n) => { a.UniqueItems(bool.Parse(n.GetScalarValue())); }
            },
            {
                "maxProperties",
                (a, n) => { a.MaxProperties(uint.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture)); }
            },
            {
                "minProperties",
                (a, n) => { a.MinProperties(uint.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture)); }
            },
            {
                "enum", (a, n) => { a.Enum(n.CreateListOfAny().Select(a => a.GetNode())); }
            },
            {
                "const", (a, n) => { a.Const(n.CreateAny().GetNode()); }
            },
            {
                "examples", (a, n) => { a.Examples(n.CreateListOfAny().Select(a => a.GetNode())); }
            },
            {
                "if", (a, n) => { a.If(LoadSchema(n)); }
            },
            {
                "then", (a, n) => { a.Then(LoadSchema(n)); }
            },
            {
                "else", (a, n) => { a.Else(LoadSchema(n)); }
            },
            {
                "readOnly", (a, n) => { a.ReadOnly(bool.Parse(n.GetScalarValue())); }
            },
            {
                "writeOnly", (a, n) => { a.WriteOnly(bool.Parse(n.GetScalarValue())); }
            },
            {
                "properties", (a, n) => { a.Properties(n.CreateMap(LoadSchema)); }
            },
            {
                "additionalProperties", (a, n) =>
                {
                    if (n is ValueNode)
                    {
                        a.AdditionalProperties(bool.Parse(n.GetScalarValue()));
                    }
                    else
                    {
                        a.AdditionalProperties(LoadSchema(n));
                    }
                }
            },
            {
                "items", (a, n) =>
                {
                    a.Items(LoadSchema(n));
                }
            },
            {
                "additionalItems", (a, n) =>
                {
                    if (n is ValueNode)
                    {
                        a.AdditionalProperties(bool.Parse(n.GetScalarValue()));
                    }
                    else
                    {
                        a.AdditionalProperties(LoadSchema(n));
                    }
                }
            },
            {
                "patternProperties", (a, n) => { a.PatternProperties(n.CreateMap(LoadSchema)); }
            },
            {
                "propertyNames", (a, n) => { a.PropertyNames(LoadSchema(n)); }
            },
            {
                "contains", (a, n) => { a.Contains(LoadSchema(n)); }
            },
            {
                "allOf", (a, n) => { a.AllOf(n.CreateList(LoadSchema)); }
            },
            {
                "oneOf", (a, n) => { a.OneOf(n.CreateList(LoadSchema)); }
            },
            {
                "anyOf", (a, n) => { a.AnyOf(n.CreateList(LoadSchema)); }
            },
            {
                "not", (a, n) => { a.Not(LoadSchema(n)); }
            },
            {
                "description", (a, n) => { a.Description(n.GetScalarValue()); }
            },
            {
                "format", (a, n) => { a.Format(n.GetScalarValue()); }
            },
            {
                "default", (a, n) => { a.Default(n.CreateAny().GetNode()); }
            },
            {
                "discriminator", (a, n) => { a.Discriminator(n.GetScalarValue()); }
            },
            {
                "externalDocs", (a, n) => { a.ExternalDocs(AsyncApiV2Deserializer.LoadExternalDocs(n)); }
            },
            {
                "deprecated", (a, n) => { a.Deprecated(bool.Parse(n.GetScalarValue())); }
            },
            {
                "nullable", (a, n) => { a.Nullable(n.GetBooleanValue()); }
            },
        };

        private static Dictionary<string, IAsyncApiExtension> LoadExtensions(string value, IAsyncApiExtension extension)
        {
            var extensions = new Dictionary<string, IAsyncApiExtension>
            {
                { value, extension },
            };
            return extensions;
        }

        private static readonly PatternFieldMap<JsonSchemaBuilder> schemaPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.Extensions(LoadExtensions(p, AsyncApiV2Deserializer.LoadExtension(p, n))) },
            };

        public static JsonSchema LoadSchema(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Schema);
            var schemaBuilder = new JsonSchemaBuilder();

            // check for a $ref and if present, add it to the builder as a Ref keyword
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return schemaBuilder.Ref(pointer);
            }

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schemaBuilder, schemaFixedFields, schemaPatternFields);
            }

            var schema = schemaBuilder.Build();
            return schema;
        }
    }
}