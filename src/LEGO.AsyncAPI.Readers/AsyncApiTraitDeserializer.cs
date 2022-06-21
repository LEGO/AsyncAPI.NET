using System;
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncAPI document into
    /// runtime AsyncAPI object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiMessageTrait> _traitFixedFields =
            new FixedFieldMap<AsyncApiMessageTrait>
            {
                {
                    "headers", (o, n) =>
                    {
                        o.Headers = LoadSchema(n);
                    }
                },
                {
                    "correlationId", (o, n) =>
                    {
                        o.CorrelationId = LoadCorrelationId(n);
                    }
                },
                {
                    "schemaFormat", (o, n) =>
                    {
                        o.SchemaFormat = n.GetScalarValue();
                    }
                },
                {
                    "contentType", (o, n) =>
                    {
                        o.ContentType = n.GetScalarValue();
                    }
                },
                {
                    "name", (o, n) =>
                    {
                        o.Name = n.GetScalarValue();
                    }
                },
                {
                    "title", (o, n) =>
                    {
                        o.Name = n.GetScalarValue();
                    }
                },
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    "tags", (a, n) => a.Tags = n.CreateList(LoadTag)
                },
                {
                    "externalDocs", (o, n) =>
                    {
                        o.ExternalDocs = LoadExternalDocs(n);
                    }
                },
                {
                    "examples", (a, n) => a.Examples = n.CreateList(LoadExample)
                },
            };

        private static readonly PatternFieldMap<AsyncApiMessageTrait> _traitPatternFields =
            new PatternFieldMap<AsyncApiMessageTrait>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiMessageTrait LoadTrait(ParseNode node)
        {
            var mapNode = node.CheckMapNode("traits");

            var trait = new AsyncApiMessageTrait();
            foreach (var property in mapNode)
            {
                property.ParseField(trait, _traitFixedFields, _traitPatternFields);
            }

            return trait;
        }
    }
}
