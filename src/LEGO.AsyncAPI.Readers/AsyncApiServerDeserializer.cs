using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiServer> _serverFixedFields = new FixedFieldMap<AsyncApiServer>
        {
            {
                "url", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "variables", (o, n) =>
                {
                    o.Variables = n.CreateMap(LoadServerVariable);
                }
            },
            {
                "security", (o, n) =>
                {
                    o.Security = n.CreateList(LoadSecurityRequirement);
                }
            },
            // { TODO, figure out bindings
            //     "bindings", (o, n) =>
            //     {
            //         o.Bindings = n.
            //     }
            // },
            {
                "protocolVersion", (o, n) =>
                {
                    o.ProtocolVersion = n.GetScalarValue();
                }
            },
            {
                "protocol", (o, n) =>
                {
                    o.Protocol = n.GetScalarValue();
                }
            },
        };

        private static readonly PatternFieldMap<AsyncApiServer> _serverPatternFields = new PatternFieldMap<AsyncApiServer>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiServer LoadServer(ParseNode node)
        {
            var mapNode = node.CheckMapNode("server");

            var server = new AsyncApiServer();

            ParseMap(mapNode, server, _serverFixedFields, _serverPatternFields);

            return server;
        }
    }
}