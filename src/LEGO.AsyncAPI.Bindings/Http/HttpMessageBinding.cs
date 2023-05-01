// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncAPI.Bindings.Http
{
    using System;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for http messaging channels.
    /// </summary>
    public class HttpMessageBinding : MessageBinding<HttpMessageBinding>
    {

        /// <summary>
        /// A Schema object containing the definitions for HTTP-specific headers. This schema MUST be of type object and have a properties key.
        /// </summary>
        public AsyncApiSchema Headers { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

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

            writer.WriteOptionalObject(AsyncApiConstants.Headers, Headers, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, BindingVersion);
            writer.WriteExtensions(Extensions);

            writer.WriteEndObject();
        }

        public override string BindingKey => "http";

        protected override FixedFieldMap<HttpMessageBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "headers", (a, n) => { a.Headers = JsonSchemaDeserializer.LoadSchema(n); } },
        };

    }
}
