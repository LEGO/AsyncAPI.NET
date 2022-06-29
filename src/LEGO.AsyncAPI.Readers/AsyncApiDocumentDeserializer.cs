// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiDocument> asyncApiFixedFields = new()
        {
            { "asyncapi", (a, n) => a.Asyncapi = n.GetScalarValue() },
            { "id", (a, n) => a.Id = n.GetScalarValue() },
            { "info", (a, n) => a.Info = LoadInfo(n) },
            { "servers", (a, n) => a.Servers = n.CreateMap(LoadServer) },
            { "defaultContentType", (a, n) => a.DefaultContentType = n.GetScalarValue() },
            { "channels", (a, n) => a.Channels = n.CreateMap(LoadChannel) },
            { "components", (a, n) => a.Components = LoadComponents(n) },
            { "tags", (a, n) => a.Tags = n.CreateList(LoadTag) },
            { "externalDocs", (a, n) => a.ExternalDocs = LoadExternalDocs(n) },
        };

        private static PatternFieldMap<AsyncApiDocument> asyncApiPatternFields = new()
        {
            { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
        };

        public static AsyncApiDocument LoadAsyncApi(RootNode rootNode)
        {
            var document = new AsyncApiDocument();

            var asyncApiNode = rootNode.GetMap();

            ParseMap(asyncApiNode, document, asyncApiFixedFields, asyncApiPatternFields);

            return document;
        }
    }
}
