// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Http
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for http operations.
    /// </summary>
    public class HttpOperationBinding : IOperationBinding
    {
        /// <summary>
        /// REQUIRED. Type of operation. Its value MUST be either request or response.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// When type is request, this is the HTTP method, otherwise it MUST be ignored. Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A Schema object containing the definitions for each query parameter. This schema MUST be of type object and have a properties key.
        /// </summary>
        public AsyncApiSchema Query { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <summary>
        /// Serialize to AsyncAPI V2 document without using reference.
        /// </summary>
        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Type, Type);
            writer.WriteOptionalProperty(AsyncApiConstants.Method, Method);
            writer.WriteOptionalObject(AsyncApiConstants.Query, Query, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, BindingVersion);

            writer.WriteEndObject();
        }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineReferences)
            {
                Reference.SerializeV2(writer);
                return;
            }

            SerializeV2WithoutReference(writer);
        }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        BindingType IBinding.Type => BindingType.Http;
    }
}
