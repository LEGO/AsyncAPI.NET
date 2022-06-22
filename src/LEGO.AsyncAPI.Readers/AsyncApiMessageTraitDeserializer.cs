using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiMessageTrait> _messageTraitFixedFields = new ()
        {
            { "headers", (a, n) => { a.Headers = LoadSchema(n); } },
            { "correlationId", (a, n) => { a.CorrelationId = LoadCorrelationId(n); } },
            { "schemaFormat", (a, n) => { a.SchemaFormat = n.GetScalarValue(); } },
            { "contentType", (a, n) => { a.ContentType = n.GetScalarValue(); } },
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "title", (a, n) => { a.Title = n.GetScalarValue(); } },
            { "summary", (a, n) => { a.Summary = n.GetScalarValue(); } },
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "tags", (a, n) => { a.Tags = n.CreateList(LoadTag); } },
            { "externalDocs", (a, n) => { a.ExternalDocs = LoadExternalDocs(n); } },
            { "bindings", (a, n) => { ; } }, // TODO: Do something with Bindings
        };

        private static PatternFieldMap<AsyncApiMessageTrait> _messageTraitPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiMessageTrait LoadMessageTrait(ParseNode node)
        {
            var mapNode = node.CheckMapNode("traits");
            var operationTrait = new AsyncApiMessageTrait();

            ParseMap(mapNode, operationTrait, _messageTraitFixedFields, _messageTraitPatternFields);

            return operationTrait;
        }
    }
}
