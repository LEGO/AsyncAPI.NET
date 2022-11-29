// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
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
            { "topicConfiguration", (a, n) => { a.TopicConfiguration = LoadTopicConfiguration(n); } },
            { "replicas", (a, n) => { a.Replicas = n.GetIntegerValue(); } },
        };

        private static FixedFieldMap<TopicConfigurationObject> kafkaChannelTopicConfigurationObjectFixedFields = new()
        {
            { "cleanup.policy", (a, n) => { a.CleanupPolicy = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "retention.ms", (a, n) => { a.RetentionMiliseconds = n.GetIntegerValue(); } },
            { "retention.bytes", (a, n) => { a.RetentionBytes = n.GetIntegerValue(); } },
            { "delete.retention.ms", (a, n) => { a.DeleteRetentionMiliseconds = n.GetIntegerValue(); } },
            { "max.message.bytes", (a, n) => { a.MaxMessageBytes = n.GetIntegerValue(); } },
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

        private static TopicConfigurationObject LoadTopicConfiguration(ParseNode node)
        {
            var mapNode = node.CheckMapNode("topicConfiguration");
            var retention = new TopicConfigurationObject();
            ParseMap(mapNode, retention, kafkaChannelTopicConfigurationObjectFixedFields, null);
            return retention;
        }
    }
}
