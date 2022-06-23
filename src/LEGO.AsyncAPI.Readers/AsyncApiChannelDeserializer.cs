using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiChannel> _channelFixedFields = new ()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "servers", (a, n) => { a.Servers = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "subscribe", (a, n) => { a.Subscribe = LoadOperation(n); } },
            { "publish", (a, n) => { a.Publish = LoadOperation(n); } },
            { "parameters", (a, n) => { a.Parameters = n.CreateMapWithReference(ReferenceType.Parameter, LoadParameter); } },
            { "bindings", (a, n) => { ; } }, //TODO: Figure out bindings
        };

        private static readonly PatternFieldMap<AsyncApiChannel> _channelPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };
        
        public static AsyncApiChannel LoadChannel(ParseNode node)
        {
            var mapNode = node.CheckMapNode("channel");

            var pathItem = new AsyncApiChannel();

            ParseMap(mapNode, pathItem, _channelFixedFields, _channelPatternFields);

            return pathItem;
        }
    }
}
