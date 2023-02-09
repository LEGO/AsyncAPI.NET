// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.WebSockets
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class WebSocketsChannelBinding : IChannelBinding
    {
        /// <summary>
        /// The HTTP method t use when establishing the connection. Its value MUST be either 'GET' or 'POST'.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// A Schema object containing the definitions for each query parameter. This schema MUST be of type 'object' and have a 'properties' key.
        /// </summary>
        public AsyncApiSchema? Query { get; set; }

        /// <summary>
        /// A Schema object containing the definitions of the HTTP headers to use when establishing the connection. This schma MUST be of type 'object' and have a 'properties' key.
        /// </summary>
        public AsyncApiSchema? Headers { get; set; }

        public string BindingVersion { get; set; }

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } =
            new Dictionary<string, IAsyncApiExtension>();

        public BindingType Type => BindingType.Websockets;

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
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

            writer.WriteEndObject();
        }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineReferences)
            {
                this.Reference.SerializeV2(writer);
                return;
            }

            this.SerializeV2WithoutReference(writer);
        }
    }
}