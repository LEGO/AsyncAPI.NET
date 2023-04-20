// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Globalization;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiV2Deserializer
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
                        a.Type = new List<SchemaType> { n.GetScalarValue().GetEnumFromDisplayName<SchemaType>() };
                    }
                    else
                    {
                        a.Type = new List<SchemaType>(n.CreateSimpleList(n2 => n2.GetScalarValue().GetEnumFromDisplayName<SchemaType>()));
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
                    a.MultipleOf = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "maximum",
                (a, n) =>
                {
                    a.Maximum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMaximum", (a, n) => { a.ExclusiveMaximum = bool.Parse(n.GetScalarValue()); }
            },
            {
                "minimum",
                (a, n) =>
                {
                    a.Minimum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
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
                "additionalProperties", (a, n) => { a.AdditionalProperties = LoadSchema(n); }
            },
            {
                "items", (a, n) => { a.Items = LoadSchema(n); }
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
                "externalDocs", (a, n) => { a.ExternalDocs = LoadExternalDocs(n); }
            },
            {
                "deprecated", (a, n) => { a.Deprecated = bool.Parse(n.GetScalarValue()); }
            },
        };

        private static readonly PatternFieldMap<AsyncApiSchema> schemaPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n)) },
            };

        private static readonly AnyFieldMap<AsyncApiSchema> schemaAnyFields = new()
        {
            {
                AsyncApiConstants.Default,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Default,
                    (s, v) => s.Default = v,
                    s => s)
            },
        };

        private static readonly AnyListFieldMap<AsyncApiSchema> schemaAnyListFields = new()
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
                return new AsyncApiSchema
                {
                    UnresolvedReference = true,
                    Reference = node.Context.VersionService.ConvertToAsyncApiReference(pointer, ReferenceType.Schema),
                };
            }

            var schema = new AsyncApiSchema();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schema, schemaFixedFields, schemaPatternFields);
            }

            ProcessAnyFields(mapNode, schema, schemaAnyFields);
            ProcessAnyListFields(mapNode, schema, schemaAnyListFields);

            return schema;
        }
    }
}