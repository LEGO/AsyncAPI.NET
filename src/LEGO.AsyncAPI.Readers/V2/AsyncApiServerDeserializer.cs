// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiServer> serverFixedFields = new()
        {
            {
                "url", (a, n) => { a.Url = n.GetScalarValue(); }
            },
            {
                "description", (a, n) => { a.Description = n.GetScalarValue(); }
            },
            {
                "variables", (a, n) => { a.Variables = n.CreateMap(LoadServerVariable); }
            },
            {
                "security", (a, n) => { a.Security = n.CreateList(LoadSecurityRequirement); }
            },
            {
                "tags", (a, n) => { a.Tags = n.CreateList(LoadTag); }
            },
            {
                "bindings", (o, n) => { o.Bindings = LoadServerBindings(n); }
            },
            {
                "protocolVersion", (a, n) => { a.ProtocolVersion = n.GetScalarValue(); }
            },
            {
                "protocol", (a, n) => { a.Protocol = n.GetScalarValue(); }
            },
        };

        private static readonly PatternFieldMap<AsyncApiServer> serverPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiServer LoadServer(ParseNode node)
        {
            var mapNode = node.CheckMapNode("servers");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiServer>(ReferenceType.Server, pointer);
            }

            var server = new AsyncApiServer();

            ParseMap(mapNode, server, serverFixedFields, serverPatternFields);

            return server;
        }
    }
}
