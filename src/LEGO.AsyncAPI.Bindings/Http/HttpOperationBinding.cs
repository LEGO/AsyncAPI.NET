// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncAPI.Bindings.Http
{
    using System;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for http operations.
    /// </summary>
    public class HttpOperationBinding : OperationBinding<HttpOperationBinding>
    {
        public enum HttpOperationType
        {
            [Display("request")]
            Request,

            [Display("response")]
            Response,
        }
        /// <summary>
        /// REQUIRED. Type of operation. Its value MUST be either request or response.
        /// </summary>
        public HttpOperationType? Type { get; set; }

        /// <summary>
        /// When type is request, this is the HTTP method, otherwise it MUST be ignored. Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A Schema object containing the definitions for each query parameter. This schema MUST be of type object and have a properties key.
        /// </summary>
        public AsyncApiSchema Query { get; set; }

        /// <summary>
        /// Serialize to AsyncAPI V2 document without using reference.
        /// </summary>
        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Type, this.Type.GetDisplayName());
            writer.WriteOptionalProperty(AsyncApiConstants.Method, Method);
            writer.WriteOptionalObject(AsyncApiConstants.Query, Query, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }

        /// <inheritdoc/>
        

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        protected override FixedFieldMap<HttpOperationBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "type", (a, n) => { a.Type = n.GetScalarValue().GetEnumFromDisplayName<HttpOperationType>(); } },
            { "method", (a, n) => { a.Method = n.GetScalarValue(); } },
            { "query", (a, n) => { a.Query = JsonSchemaDeserializer.LoadSchema(n); } },
        };

        public override string BindingKey => "http";
    }
}
