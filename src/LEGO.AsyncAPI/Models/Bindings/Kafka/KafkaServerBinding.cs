namespace LEGO.AsyncAPI.Models.Bindings.Kafka
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Kafka server settings.
    /// </summary>
    public class KafkaServerBinding : IServerBinding
    {
        /// <summary>
        /// API URL for the Schema Registry used when producing Kafka messages (if a Schema Registry was used)
        /// </summary>
        public string SchemaRegistryUrl { get; set; }

        /// <summary>
        /// The vendor of Schema Registry and Kafka serdes library that should be used (e.g. apicurio, confluent, ibm, or karapace)
        /// </summary>
        public string SchemaRegistryVendor { get; set; }

        /// <summary>
        /// The version of this binding.
        /// </summary>
        public string BindingVersion { get; set; }

        public BindingType Type => BindingType.Kafka;

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

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
            writer.WriteProperty(AsyncApiConstants.SchemaRegistryUrl, SchemaRegistryUrl);
            writer.WriteProperty(AsyncApiConstants.SchemaRegistryVendor, SchemaRegistryVendor);
            writer.WriteProperty(AsyncApiConstants.BindingVersion, BindingVersion);

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
    }
}
