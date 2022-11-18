namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<KafkaServerBinding> kafkaServerBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "schemaRegistryUrl", (a, n) => { a.SchemaRegistryUrl = n.GetScalarValue(); } },
            { "schemaRegistryVendor", (a, n) => { a.SchemaRegistryVendor = n.GetScalarValue(); } },
        };

        private static FixedFieldMap<KafkaChannelBinding> kafkaChannelBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "topic", (a, n) => { a.Topic = n.GetScalarValue(); } },
            { "partitions", (a, n) => { a.Partitions = n.GetIntegerValue(); } },
            { "replicas", (a, n) => { a.Replicas = n.GetIntegerValue(); } },
        };

        private static FixedFieldMap<KafkaOperationBinding> kafkaOperationBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "groupId", (a, n) => { a.GroupId = LoadSchema(n); } },
            { "clientId", (a, n) => { a.ClientId = LoadSchema(n); } },
        };

        private static FixedFieldMap<KafkaMessageBinding> kafkaMessageBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "key", (a, n) => { a.Key = LoadSchema(n); } },
            { "schemaIdLocation", (a, n) => { a.SchemaIdLocation = n.GetScalarValue(); } },
            { "schemaIdPayloadEncoding", (a, n) => { a.SchemaIdPayloadEncoding = n.GetScalarValue(); } },
            { "schemaLookupStrategy", (a, n) => { a.SchemaLookupStrategy = n.GetScalarValue(); } },
        };
    }
}
