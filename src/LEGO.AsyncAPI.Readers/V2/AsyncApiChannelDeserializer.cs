// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiChannel> ChannelFixedFields = new()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "servers", (a, n) => { a.Servers = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "subscribe", (a, n) => { a.Subscribe = LoadOperation(n); } },
            { "publish", (a, n) => { a.Publish = LoadOperation(n); } },
            { "parameters", (a, n) => { a.Parameters = n.CreateMap(LoadParameter); } },
            { "bindings", (a, n) => { a.Bindings = LoadChannelBindings(n); } },
        };

        private static readonly PatternFieldMap<AsyncApiChannel> ChannelPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiChannel LoadChannel(ParseNode node)
        {
            var mapNode = node.CheckMapNode("channel");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return new AsyncApiChannelReference(pointer);
            }

            var pathItem = new AsyncApiChannel();

            ParseMap(mapNode, pathItem, ChannelFixedFields, ChannelPatternFields);

            return pathItem;
        }
    }
}
