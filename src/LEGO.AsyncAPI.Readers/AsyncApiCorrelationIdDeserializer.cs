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
        private static readonly FixedFieldMap<AsyncApiCorrelationId> _correlationIdFixedFileds =
            new FixedFieldMap<AsyncApiCorrelationId>
            {
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    "location", (o, n) =>
                    {
                        o.Location = n.GetScalarValue();
                    }
                },
            };

        private static readonly PatternFieldMap<AsyncApiCorrelationId> _correlationIdPatternFields =
            new PatternFieldMap<AsyncApiCorrelationId>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiCorrelationId LoadCorrelationId(ParseNode node)
        {
            var mapNode = node.CheckMapNode("correlationId");

            var correlationId = new AsyncApiCorrelationId();
            foreach (var property in mapNode)
            {
                property.ParseField(correlationId, _correlationIdFixedFileds, _correlationIdPatternFields);
            }

            return correlationId;
        }
    }
}
