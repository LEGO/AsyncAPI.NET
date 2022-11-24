// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Kafka
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Kafka channel settings.
    /// </summary>
    public class KafkaChannelBinding : IChannelBinding
    {
        /// <summary>
        /// Kafka topic name if different from channel name.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Number of partitions configured on this topic (useful to know how many parallel consumers you may run).
        /// </summary>
        public int Partitions { get; set; }

        /// <summary>
        /// Number of replicas configured on this topic.
        /// </summary>
        public int Replicas { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
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
            writer.WriteOptionalProperty(AsyncApiConstants.Topic, Topic);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.Partitions, Partitions);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.Replicas, Replicas);
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
    }
}
