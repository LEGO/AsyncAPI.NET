using System.Collections.ObjectModel;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    public class AsyncApiChannelDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiChannel> _channelFixedFields = new ()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "servers", (a, n) => { a.Servers = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "subscribe", (a, n) => { a.Subscribe = LoadSubscription(n); } },
            { "publish", (a, n) => { a.Publish = LoadPublisher(n); } },
            { "parameters", (a, n) => { a.Parameters = LoadParameters(n); } },
            { "bindings", (a, n) => { a.Bindings = LoadBindings(n); } },
        };

        private static readonly PatternFieldMap<AsyncApiChannel> _pathItemPatternFields =
            new PatternFieldMap<AsyncApiChannel>
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };
        
        public static AsyncApiChannel LoadChannel(ParseNode node)
        {
            var mapNode = node.CheckMapNode("PathItem");

            var pathItem = new AsyncApiChannel();

            ParseMap(mapNode, pathItem, _pathItemFixedFields, _pathItemPatternFields);

            return pathItem;
        }
    }
}