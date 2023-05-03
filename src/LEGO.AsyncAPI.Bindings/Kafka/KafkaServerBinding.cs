// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Kafka
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Kafka server settings.
    /// </summary>
    public class KafkaServerBinding : ServerBinding<KafkaServerBinding>
    {
        /// <summary>
        /// API URL for the Schema Registry used when producing Kafka messages (if a Schema Registry was used)
        /// </summary>
        public string SchemaRegistryUrl { get; set; }

        /// <summary>
        /// The vendor of Schema Registry and Kafka serdes library that should be used (e.g. apicurio, confluent, ibm, or karapace)
        /// </summary>
        public string SchemaRegistryVendor { get; set; }


        public override string BindingKey => "kafka";

        protected override FixedFieldMap<KafkaServerBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "schemaRegistryUrl", (a, n) => { a.SchemaRegistryUrl = n.GetScalarValue(); } },
            { "schemaRegistryVendor", (a, n) => { a.SchemaRegistryVendor = n.GetScalarValue(); } },
        };

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
            writer.WriteOptionalProperty(AsyncApiConstants.SchemaRegistryUrl, this.SchemaRegistryUrl);
            writer.WriteOptionalProperty(AsyncApiConstants.SchemaRegistryVendor, this.SchemaRegistryVendor);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}
