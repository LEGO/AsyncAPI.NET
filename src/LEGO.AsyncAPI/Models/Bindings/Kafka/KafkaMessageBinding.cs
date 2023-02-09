// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Kafka
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for kafka messages.
    /// </summary>
    public class KafkaMessageBinding : IMessageBinding
    {
        /// <summary>
        /// The message key. NOTE: You can also use the <a href="https://www.asyncapi.com/docs/reference/specification/v2.4.0#referenceObject">reference object</a> way.
        /// </summary>
        public AsyncApiSchema? Key { get; set; }

        /// <summary>
        /// If a Schema Registry is used when performing this operation, tells where the id of schema is stored (e.g. header or payload).
        /// </summary>
        public string? SchemaIdLocation { get; set; }

        /// <summary>
        /// Number of bytes or vendor specific values when schema id is encoded in payload (e.g confluent/ apicurio-legacy / apicurio-new).
        /// </summary>
        public string? SchemaIdPayloadEncoding { get; set; }

        /// <summary>
        /// Freeform string for any naming strategy class to use. Clients should default to the vendor default if not supplied.
        /// </summary>
        public string? SchemaLookupStrategy { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>
        public string? BindingVersion { get; set; }

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data.
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

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

            writer.WriteOptionalObject(AsyncApiConstants.Key, this.Key, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.SchemaIdLocation, this.SchemaIdLocation);
            writer.WriteOptionalProperty(AsyncApiConstants.SchemaIdPayloadEncoding, this.SchemaIdPayloadEncoding);
            writer.WriteOptionalProperty(AsyncApiConstants.SchemaLookupStrategy, this.SchemaLookupStrategy);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the v2.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">writer</exception>
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

        /// <summary>
        /// Gets or sets this object MAY be extended with Specification Extensions.
        /// To protect the API from leaking the underlying JSON library, the extension data extraction is handled by a customer resolver.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public BindingType Type => BindingType.Kafka;
    }
}
