using System.Collections.Generic;
using System.Globalization;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncApi.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiSchema> _schemaFixedFields = new()
        {
            {
                "title", (a, n) =>
                {
                    a.Title = n.GetScalarValue();
                }
            },
            {
                "multipleOf", (a, n) =>
                {
                    a.MultipleOf = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture); 
                }
            },
            {
                "maximum", (a, n) =>
                {
                    a.Maximum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMaximum", (a, n) =>
                {
                    a.ExclusiveMaximum = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "minimum", (a, n) =>
                {
                    a.Minimum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMinimum", (a, n) =>
                {
                    a.ExclusiveMinimum = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "maxLength", (a, n) =>
                {
                    a.MaxLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "minLength", (a, n) =>
                {
                    a.MinLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "pattern", (a, n) =>
                {
                    a.Pattern = n.GetScalarValue();
                }
            },
            {
                "maxItems", (a, n) =>
                {
                    a.MaxItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "minItems", (a, n) =>
                {
                    a.MinItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "uniqueItems", (a, n) =>
                {
                    a.UniqueItems = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "maxProperties", (a, n) =>
                {
                    a.MaxProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "minProperties", (a, n) =>
                {
                    a.MinProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "required", (a, n) =>
                {
                    a.Required = new HashSet<string>(n.CreateSimpleList(n2 => n2.GetScalarValue()));
                }
            },
            {
                "enum", (a, n) =>
                {
                    a.Enum = n.CreateListOfAny();
                }
            },
            {
                "type", (a, n) =>
                {
                    a.Type = n.GetScalarValue();
                }
            },
            {
                "allOf", (a, n) =>
                {
                    a.AllOf = n.CreateList(LoadSchema);
                }
            },
            {
                "oneOf", (a, n) =>
                {
                    a.OneOf = n.CreateList(LoadSchema);
                }
            },
            {
                "anyOf", (a, n) =>
                {
                    a.AnyOf = n.CreateList(LoadSchema);
                }
            },
            {
                "not", (a, n) =>
                {
                    a.Not = LoadSchema(n);
                }
            },
            {
                "items", (a, n) =>
                {
                    a.Items = LoadSchema(n);
                }
            },
            {
                "properties", (a, n) =>
                {
                    a.Properties = n.CreateMap(LoadSchema);
                }
            },
            {
                "additionalProperties", (a, n) =>
                {
                    if (n is ValueNode)
                    {
                        a.AdditionalPropertiesAllowed = bool.Parse(n.GetScalarValue());
                    }
                    else
                    {
                        a.AdditionalProperties = LoadSchema(n);
                    }
                }
            },
            {
                "description", (a, n) =>
                {
                    a.Description = n.GetScalarValue();
                }
            },
            {
                "format", (a, n) =>
                {
                    a.Format = n.GetScalarValue();
                }
            },
            {
                "default", (a, n) =>
                {
                    a.Default = n.CreateAny();
                }
            },

            {
                "nullable", (a, n) =>
                {
                    a.Nullable = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "discriminator", (a, n) =>
                {
                    a.Discriminator = LoadDiscriminator(n);
                }
            },
            {
                "readOnly", (a, n) =>
                {
                    a.ReadOnly = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "writeOnly", (a, n) =>
                {
                    a.WriteOnly = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "xml", (a, n) =>
                {
                    a.Xml = LoadXml(n);
                }
            },
            {
                "externalDocs", (a, n) =>
                {
                    a.ExternalDocs = LoadExternalDocs(n);
                }
            },
            {
                "example", (a, n) =>
                {
                    a.Example = n.CreateAny();
                }
            },
            {
                "deprecated", (a, n) =>
                {
                    a.Deprecated = bool.Parse(n.GetScalarValue());
                }
            },
        };

        private static readonly PatternFieldMap<AsyncApiSchema> _schemaPatternFields = new PatternFieldMap<AsyncApiSchema>
        {
            {s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p,n))}
        };

        private static readonly AnyFieldMap<AsyncApiSchema> _schemaAnyFields = new AnyFieldMap<AsyncApiSchema>
        {
            {
                AsyncApiConstants.Default,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Default,
                    (s, v) => s.Default = v,
                    s => s)
            },
            {
                 AsyncApiConstants.Example,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Example,
                    (s, v) => s.Example = v,
                    s => s)
            }
        };

        private static readonly AnyListFieldMap<AsyncApiSchema> _schemaAnyListFields = new()
        {
            {
                AsyncApiConstants.Enum,
                new AnyListFieldMapParameter<AsyncApiSchema>(
                    s => s.Enum,
                    (s, v) => s.Enum = v,
                    s => s)
            }
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
                    Reference = node.Context.VersionService.ConvertToAsyncApiReference(pointer, ReferenceType.Schema)
                };
            }

            var schema = new AsyncApiSchema();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schema, _schemaFixedFields, _schemaPatternFields);
            }

            ProcessAnyFields(mapNode, schema, _schemaAnyFields);
            ProcessAnyListFields(mapNode, schema, _schemaAnyListFields);

            return schema;
        }
    }
}