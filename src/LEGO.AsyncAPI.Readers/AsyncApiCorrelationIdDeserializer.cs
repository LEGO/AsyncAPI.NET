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
            new ()
            { 
                { "description", (a, n) => { a.Description = n.GetScalarValue(); } }, 
                { "location", (a, n) => { a.Location = n.GetScalarValue(); } },
            };

        private static readonly PatternFieldMap<AsyncApiCorrelationId> _correlationIdPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
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
