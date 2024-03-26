// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Http
{
    using System;
    using Json.Schema;
    using LEGO.AsyncAPI.Models;
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
        public JsonSchema Headers { get; set; }

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

            writer.WriteOptionalObject(AsyncApiConstants.Headers, this.Headers, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);

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
