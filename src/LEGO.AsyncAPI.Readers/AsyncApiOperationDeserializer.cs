using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiOperation> _operationFixedFields =
            new ()
            {
                {
                    "operationId", (a, n) => { a.OperationId = n.GetScalarValue(); }
                },
                {
                    "summary", (a, n) => { a.Summary = n.GetScalarValue(); }
                },
                {
                    "description", (a, n) => { a.Description = n.GetScalarValue(); }
                },
                {
                    "tags", (a, n) => a.Tags = LoadTags(n)
                },
                {
                    "externalDocs", (a, n) => { a.ExternalDocs = LoadExternalDocs(n); }
                },
                {
                    "bindings", (a, n) => { a.Bindings = LoadBindings(n); }
                },
                {
                    "traits", (a, n) => { a.Traits = LoadTraits(n); }
                },
                { "message", (a, n) => { a.Message = LoadMessage(n); } }
            };

        private static readonly PatternFieldMap<AsyncApiOperation> _operationPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n)) },
            };

        internal static AsyncApiOperation LoadOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation");

            var operation = new AsyncApiOperation();

            ParseMap(mapNode, operation, _operationFixedFields, _operationPatternFields);

            return operation;
        }

        private static AsyncApiTag LoadTagByReference(
            ParsingContext context,
            string tagName)
        {
            var tagObject = new AsyncApiTag
            {
                UnresolvedReference = true,
                Reference = new AsyncApiReference
                {
                    Type = ReferenceType.Tag,
                    Id = tagName
                }
            };

            return tagObject;
        }
    }
}