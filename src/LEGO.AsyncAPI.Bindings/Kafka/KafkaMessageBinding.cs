// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Kafka
{
    using System;
    using Json.Schema;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for kafka messages.
    /// </summary>
    public class KafkaMessageBinding : MessageBinding<KafkaMessageBinding>
    {
        /// <summary>
        /// The message key. NOTE: You can also use the <a href="https://www.asyncapi.com/docs/reference/specification/v2.4.0#referenceObject">reference object</a> way.
        /// </summary>
        public JsonSchema Key { get; set; }

        /// <summary>
        /// If a Schema Registry is used when performing this operation, tells where the id of schema is stored (e.g. header or payload).
        /// </summary>
        public string SchemaIdLocation { get; set; }

        /// <summary>
        /// Number of bytes or vendor specific values when schema id is encoded in payload (e.g confluent/ apicurio-legacy / apicurio-new).
        /// </summary>
        public string SchemaIdPayloadEncoding { get; set; }

        /// <summary>
        /// Freeform string for any naming strategy class to use. Clients should default to the vendor default if not supplied.
        /// </summary>
        public string SchemaLookupStrategy { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>

        public override void SerializeProperties(IAsyncApiWriter writer)
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
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the v2.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">writer.</exception>

        public override string BindingKey => "kafka";

        protected override FixedFieldMap<KafkaMessageBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "key", (a, n) => { a.Key = JsonSchemaDeserializer.LoadSchema(n); } },
            { "schemaIdLocation", (a, n) => { a.SchemaIdLocation = n.GetScalarValue(); } },
            { "schemaIdPayloadEncoding", (a, n) => { a.SchemaIdPayloadEncoding = n.GetScalarValue(); } },
            { "schemaLookupStrategy", (a, n) => { a.SchemaLookupStrategy = n.GetScalarValue(); } },
        };
    }
}
