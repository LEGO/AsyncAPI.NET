// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models.Bindings.Http;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using Models.Bindings.WebSockets;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<WebSocketsChannelBinding> webSocketsChannelBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "method", (a, n) => { a.Method = n.GetScalarValue(); } },
            { "query", (a, n) => { a.Query = LoadSchema(n); } },
            { "headers", (a, n) => { a.Headers = LoadSchema(n); } },
        };
    }
}
