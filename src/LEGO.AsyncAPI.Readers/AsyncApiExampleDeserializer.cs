using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiMessageExample> _exampleFixedFields = new ()
        {
            { "headers", (a, n) => { a.Headers = n.CreateSimpleMap(LoadRuntimeExpressionAnyWrapper); } },
            { "payload", (a, n) => { a.Payload = n.CreateAny(); } },
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "summary", (a, n) => { a.Summary = n.GetScalarValue(); } },
        };
        
        private static PatternFieldMap<AsyncApiMessageExample> _examplePatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiMessageExample LoadExample(ParseNode node)
        {
            var mapNode = node.CheckMapNode("example");
            var example = new AsyncApiMessageExample();

            ParseMap(mapNode, example, _exampleFixedFields, _examplePatternFields);

            return example;
        }
    }
}
