// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Kafka
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Kafka channel settings.
    /// </summary>
    public class KafkaChannelBinding : ChannelBinding<KafkaChannelBinding>
    {
        /// <summary>
        /// Kafka topic name if different from channel name.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Number of partitions configured on this topic (useful to know how many parallel consumers you may run).
        /// </summary>
        public int? Partitions { get; set; }

        /// <summary>
        /// Number of replicas configured on this topic.
        /// </summary>
        public int? Replicas { get; set; }

        /// <summary>
        /// Topic configuration properties that are relevant for the API.
        /// </summary>
        public TopicConfigurationObject TopicConfiguration { get; set; }

        public override string BindingKey => "kafka";

        protected override FixedFieldMap<KafkaChannelBinding> FixedFieldMap => new ()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "topic", (a, n) => { a.Topic = n.GetScalarValue(); } },
            { "partitions", (a, n) => { a.Partitions = n.GetIntegerValue(); } },
            { "topicConfiguration", (a, n) => { a.TopicConfiguration = n.ParseMap(kafkaChannelTopicConfigurationObjectFixedFields); } },
            { "replicas", (a, n) => { a.Replicas = n.GetIntegerValue(); } },
        };

        private static FixedFieldMap<TopicConfigurationObject> kafkaChannelTopicConfigurationObjectFixedFields = new ()
        {
            { "cleanup.policy", (a, n) => { a.CleanupPolicy = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "retention.ms", (a, n) => { a.RetentionMilliseconds = n.GetLongValue(); } },
            { "retention.bytes", (a, n) => { a.RetentionBytes = n.GetIntegerValue(); } },
            { "delete.retention.ms", (a, n) => { a.DeleteRetentionMilliseconds = n.GetIntegerValue(); } },
            { "max.message.bytes", (a, n) => { a.MaxMessageBytes = n.GetIntegerValue(); } },
            { "confluent.key.schema.validation", (a, n) => { a.ConfluentKeySchemaValidation = n.GetBooleanValue(); } },
            { "confluent.key.subject.name.strategy", (a, n) => { a.ConfluentKeySubjectName = n.GetScalarValue(); } },
            { "confluent.value.schema.validation", (a, n) => { a.ConfluentValueSchemaValidation = n.GetBooleanValue(); } },
            { "confluent.value.subject.name.strategy", (a, n) => { a.ConfluentValueSubjectName = n.GetScalarValue(); } },
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
            writer.WriteOptionalProperty(AsyncApiConstants.Topic, this.Topic);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.Partitions, this.Partitions);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.Replicas, this.Replicas);
            writer.WriteOptionalObject(AsyncApiConstants.TopicConfiguration, this.TopicConfiguration, (w, t) => t.Serialize(w));
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }
    }
}
