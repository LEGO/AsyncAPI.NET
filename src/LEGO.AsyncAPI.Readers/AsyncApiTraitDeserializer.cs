using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiOperationTrait> _operationTraitFixedFields = new ()
        {
            { "operationId", (a, n) => { a.OperationId = n.GetScalarValue(); } },
            { "summary", (a, n) => { a.Summary = n.GetScalarValue(); } },
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "tags", (a, n) => { a.Tags = n.CreateList(LoadTag); } },
            { "externalDocs", (a, n) => { a.Tags = n.CreateList(LoadTag); } },
            { "bindings", (a, n) => { ; } }, //TODO: Figure out bindings
        };

        private static PatternFieldMap<AsyncApiOperationTrait> _operationTraitPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiOperationTrait LoadOperationTrait(ParseNode node)
        {
            var mapNode = node.CheckMapNode("traits");
            var operationTrait = new AsyncApiOperationTrait();

            ParseMap(mapNode, operationTrait, _operationTraitFixedFields, _operationTraitPatternFields);

            return operationTrait;
        }
    }
}