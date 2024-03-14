// Copyright (c) The LEGO Group. All rights reserved.

using System.IO;

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Globalization;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    public class JsonSchemaDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiSchema> schemaFixedFields = new ()
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
                    a.MultipleOf = double.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "maximum",
                (a, n) =>
                {
                    a.Maximum = double.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMaximum", (a, n) => { a.ExclusiveMaximum = bool.Parse(n.GetScalarValue()); }
            },
            {
                "minimum",
                (a, n) =>
                {
                    a.Minimum = double.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMinimum", (a, n) => { a.ExclusiveMinimum = bool.Parse(n.GetScalarValue()); }
            },
            {
                "maxLength", (a, n) => { a.MaxLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture); }
            },
            {
                "minLength", (a, n) => { a.MinLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture); }
            },
            {
                "pattern", (a, n) => { a.Pattern = n.GetScalarValue(); }
            },
            {
                "maxItems", (a, n) => { a.MaxItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture); }
            },
            {
                "minItems", (a, n) => { a.MinItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture); }
            },
            {
                "uniqueItems", (a, n) => { a.UniqueItems = bool.Parse(n.GetScalarValue()); }
            },
            {
                "maxProperties",
                (a, n) => { a.MaxProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture); }
            },
            {
                "minProperties",
                (a, n) => { a.MinProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture); }
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

        private static readonly PatternFieldMap<AsyncApiSchema> schemaPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, AsyncApiV2Deserializer.LoadExtension(p, n)) },
            };

        private static readonly AnyFieldMap<AsyncApiSchema> schemaAnyFields = new ()
        {
            {
                AsyncApiConstants.Default,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Default,
                    (s, v) => s.Default = v,
                    s => s)
            },
        };

        private static readonly AnyListFieldMap<AsyncApiSchema> schemaAnyListFields = new ()
        {
            {
                AsyncApiConstants.Enum,
                new AnyListFieldMapParameter<AsyncApiSchema>(
                    s => s.Enum,
                    (s, v) => s.Enum = v,
                    s => s)
            },
        };

        public static AsyncApiSchema LoadSchema(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Schema);

            var pointer = mapNode.GetReferencePointer();

            if (pointer != null)
            {
                if (pointer.StartsWith("#"))
                {
                    return new AsyncApiSchema
                    {
                        UnresolvedReference = true,
                        Reference = node.Context.VersionService.ConvertToAsyncApiReference(pointer, ReferenceType.Schema),
                    };
                }

                var referencedContent = File.ReadAllText(pointer);
                return new AsyncApiStringReader().ReadFragment<AsyncApiSchema>(referencedContent, AsyncApiVersion.AsyncApi2_0, out _);
            }

            var schema = new AsyncApiSchema();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schema, schemaFixedFields, schemaPatternFields);
            }

            AsyncApiV2Deserializer.ProcessAnyFields(mapNode, schema, schemaAnyFields);
            AsyncApiV2Deserializer.ProcessAnyListFields(mapNode, schema, schemaAnyListFields);

            return schema;
        }
    }
}