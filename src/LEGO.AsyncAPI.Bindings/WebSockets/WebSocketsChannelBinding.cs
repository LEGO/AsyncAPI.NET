// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.WebSockets
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    public class WebSocketsChannelBinding : ChannelBinding<WebSocketsChannelBinding>
    {
        /// <summary>
        /// The HTTP method t use when establishing the connection. Its value MUST be either 'GET' or 'POST'.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A Schema object containing the definitions for each query parameter. This schema MUST be of type 'object' and have a 'properties' key.
        /// </summary>
        public AsyncApiSchema Query { get; set; }

        /// <summary>
        /// A Schema object containing the definitions of the HTTP headers to use when establishing the connection. This schma MUST be of type 'object' and have a 'properties' key.
        /// </summary>
        public AsyncApiSchema Headers { get; set; }

        public override string BindingKey => "websockets";

        protected override FixedFieldMap<WebSocketsChannelBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "method", (a, n) => { a.Method = n.GetScalarValue(); } },
            { "query", (a, n) => { a.Query = AsyncApiSchemaDeserializer.LoadSchema(n); } },
            { "headers", (a, n) => { a.Headers = AsyncApiSchemaDeserializer.LoadSchema(n); } },
        };

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteOptionalProperty(AsyncApiConstants.Method, this.Method);
            writer.WriteOptionalObject(AsyncApiConstants.Query, this.Query, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.Headers, this.Headers, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}
