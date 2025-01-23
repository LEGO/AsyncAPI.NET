// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Globalization;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiSchemaDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiJsonSchema> schemaFixedFields = new()
        {
            {
                "title", (a, n) => { a.Title = n.GetScalarValue(); }
            },
            {
                "type", (a, n) =>
                {
                    if (n.GetType() == typeof(ValueNode))
                    {
                        a.Type = n.GetScalarValue().GetEnumFromDisplayName<SchemaType>();
                    }

                    if (n.GetType() == typeof(ListNode))
                    {
                        SchemaType? initialValue = null;
                        foreach (var node in n as ListNode)
                        {
                            if (initialValue == null)
                            {
                                initialValue = node.GetScalarValue().GetEnumFromDisplayName<SchemaType>();
                                continue;
                            }

                            initialValue |= node.GetScalarValue().GetEnumFromDisplayName<SchemaType>();
                        }

                        a.Type = initialValue;
                    }
                }
            },
            {
                "required",
                (a, n) => { a.Required = new HashSet<string>(n.CreateSimpleList(n2 => n2.GetScalarValue())); }
            },
            {
                "multipleOf",
                (a, n) =>
                {
                    a.MultipleOf = double.Parse(n.GetScalarValue(), NumberStyles.Float, n.Context.Settings.CultureInfo);
                }
            },
            {
                "maximum",
                (a, n) =>
                {
                    a.Maximum = double.Parse(n.GetScalarValue(), NumberStyles.Float, n.Context.Settings.CultureInfo);
                }
            },
            {
                "exclusiveMaximum", (a, n) =>
                {
                    a.ExclusiveMaximum = double.Parse(n.GetScalarValue(), NumberStyles.Float, n.Context.Settings.CultureInfo);
                }
            },
            {
                "minimum",
                (a, n) =>
                {
                    a.Minimum = double.Parse(n.GetScalarValue(), NumberStyles.Float, n.Context.Settings.CultureInfo);
                }
            },
            {
                "exclusiveMinimum", (a, n) =>
                {
                    a.ExclusiveMinimum = double.Parse(n.GetScalarValue(), NumberStyles.Float, n.Context.Settings.CultureInfo);
                }
            },
            {
                "maxLength", (a, n) => { a.MaxLength = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo); }
            },
            {
                "minLength", (a, n) => { a.MinLength = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo); }
            },
            {
                "pattern", (a, n) => { a.Pattern = n.GetScalarValue(); }
            },
            {
                "maxItems", (a, n) => { a.MaxItems = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo); }
            },
            {
                "minItems", (a, n) => { a.MinItems = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo); }
            },
            {
                "uniqueItems", (a, n) => { a.UniqueItems = bool.Parse(n.GetScalarValue()); }
            },
            {
                "maxProperties",
                (a, n) => { a.MaxProperties = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo); }
            },
            {
                "minProperties",
                (a, n) => { a.MinProperties = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo); }
            },
            {
                "enum", (a, n) => { a.Enum = n.CreateListOfAny(); }
            },
            {
                "const", (a, n) => { a.Const = n.CreateAny(); }
            },
            {
                "examples", (a, n) => { a.Examples = n.CreateListOfAny(); }
            },
            {
                "if", (a, n) => { a.If = LoadSchema(n); }
            },
            {
                "then", (a, n) => { a.Then = LoadSchema(n); }
            },
            {
                "else", (a, n) => { a.Else = LoadSchema(n); }
            },
            {
                "readOnly", (a, n) => { a.ReadOnly = bool.Parse(n.GetScalarValue()); }
            },
            {
                "writeOnly", (a, n) => { a.WriteOnly = bool.Parse(n.GetScalarValue()); }
            },
            {
                "properties", (a, n) => { a.Properties = n.CreateMap(LoadSchema); }
            },
            {
                "additionalProperties", (a, n) =>
                {
                    if (n is ValueNode && n.GetBooleanValueOrDefault(null) == false)
                    {
                        a.AdditionalProperties = new FalseApiSchema();
                    }
                    else
                    {
                        a.AdditionalProperties = LoadSchema(n);
                    }
                }
            },
            {
                "items", (a, n) =>
                {
                    if (n is ValueNode && n.GetBooleanValueOrDefault(null) == false)
                    {
                        a.Items = new FalseApiSchema();
                    }
                    else
                    {
                        a.Items = LoadSchema(n);
                    }
                }
            },
            {
                "additionalItems", (a, n) =>
                {
                    if (n is ValueNode && n.GetBooleanValueOrDefault(null) == false)
                    {
                        a.AdditionalItems = new FalseApiSchema();
                    }
                    else
                    {
                        a.AdditionalItems = LoadSchema(n);
                    }
                }
            },
            {
                "patternProperties", (a, n) => { a.PatternProperties = n.CreateMap(LoadSchema); }
            },
            {
                "propertyNames", (a, n) => { a.PropertyNames = LoadSchema(n); }
            },
            {
                "contains", (a, n) => { a.Contains = LoadSchema(n); }
            },
            {
                "allOf", (a, n) => { a.AllOf = n.CreateList(LoadSchema); }
            },
            {
                "oneOf", (a, n) => { a.OneOf = n.CreateList(LoadSchema); }
            },
            {
                "anyOf", (a, n) => { a.AnyOf = n.CreateList(LoadSchema); }
            },
            {
                "not", (a, n) => { a.Not = LoadSchema(n); }
            },
            {
                "description", (a, n) => { a.Description = n.GetScalarValue(); }
            },
            {
                "format", (a, n) => { a.Format = n.GetScalarValue(); }
            },
            {
                "default", (a, n) => { a.Default = n.CreateAny(); }
            },
            {
                "discriminator", (a, n) => { a.Discriminator = n.GetScalarValue(); }
            },
            {
                "externalDocs", (a, n) => { a.ExternalDocs = AsyncApiV2Deserializer.LoadExternalDocs(n); }
            },
            {
                "deprecated", (a, n) => { a.Deprecated = bool.Parse(n.GetScalarValue()); }
            },
            {
                "nullable", (a, n) => { a.Nullable = n.GetBooleanValue(); }
            },
        };

        private static readonly PatternFieldMap<AsyncApiJsonSchema> schemaPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, AsyncApiV2Deserializer.LoadExtension(p, n)) },
            };

        public static AsyncApiJsonSchema LoadSchema(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Schema);

            var pointer = mapNode.GetReferencePointer();

            if (pointer != null)
            {
                return new AsyncApiJsonSchema
                {
                    UnresolvedReference = true,
                    Reference = node.Context.VersionService.ConvertToAsyncApiReference(pointer, ReferenceType.Schema),
                };
            }

            var schema = new AsyncApiJsonSchema();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schema, schemaFixedFields, schemaPatternFields);
            }

            return schema;
        }
    }
}