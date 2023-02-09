// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Pulsar
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Pulsar server settings.
    /// </summary>
    public class PulsarChannelBinding : IChannelBinding
    {
        /// <summary>
        /// The namespace associated with the topic.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// persistence of the topic in Pulsar persistent or non-persistent.
        /// </summary>
        public Persistence Persistence { get; set; }

        /// <summary>
        /// Topic compaction threshold given in bytes.
        /// </summary>
        public int? Compaction { get; set; }

        /// <summary>
        /// A list of clusters the topic is replicated to.
        /// </summary>
        public IEnumerable<string> GeoReplication { get; set; }

        /// <summary>
        /// Topic retention policy.
        /// </summary>
        public RetentionDefinition? Retention { get; set; }

        /// <summary>
        /// Message Time-to-live in seconds. 
        /// </summary>
        public int? TTL { get; set; }

        /// <summary>
        /// When Message deduplication is enabled, it ensures that each message produced on Pulsar topics is persisted to disk only once.
        /// </summary>
        public bool? Deduplication { get; set; }

        /// <summary>
        /// The version of this binding.
        public string? BindingVersion { get; set; }

        public BindingType Type => BindingType.Pulsar;

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
            writer.WriteRequiredProperty(AsyncApiConstants.Namespace, this.Namespace);
            writer.WriteRequiredProperty(AsyncApiConstants.Persistence, this.Persistence.GetDisplayName());
            writer.WriteOptionalProperty<int>(AsyncApiConstants.Compaction, this.Compaction);
            writer.WriteOptionalCollection(AsyncApiConstants.GeoReplication, this.GeoReplication, (v, s) => v.WriteValue(s));
            writer.WriteOptionalObject(AsyncApiConstants.Retention, this.Retention, (w, r) => r.Serialize(w));
            writer.WriteOptionalProperty<int>(AsyncApiConstants.TTL, this.TTL);
            writer.WriteOptionalProperty(AsyncApiConstants.Deduplication, this.Deduplication);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);

            writer.WriteEndObject();
        }

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
    }
}
