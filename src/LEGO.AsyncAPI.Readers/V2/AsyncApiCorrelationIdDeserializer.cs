// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncAPI document into
    /// runtime AsyncAPI object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiCorrelationId> correlationIdFixedFileds =
            new()
            {
                { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
                { "location", (a, n) => { a.Location = n.GetScalarValue(); } },
            };

        private static readonly PatternFieldMap<AsyncApiCorrelationId> correlationIdPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiCorrelationId LoadCorrelationId(ParseNode node)
        {
            var mapNode = node.CheckMapNode("correlationId");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return new AsyncApiCorrelationIdReference(pointer);
            }

            var correlationId = new AsyncApiCorrelationId();
            foreach (var property in mapNode)
            {
                property.ParseField(correlationId, correlationIdFixedFileds, correlationIdPatternFields);
            }

            return correlationId;
        }
    }
}
