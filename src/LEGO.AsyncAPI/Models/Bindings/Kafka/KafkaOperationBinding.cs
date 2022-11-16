// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Kafka
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Kafka operations.
    /// </summary>
    public class KafkaOperationBinding : IOperationBinding
    {
        /// <summary>
        /// Id of the consumer group.
        /// </summary>
        public AsyncApiSchema GroupId { get; set; }

        /// <summary>
        /// Id of the consumer inside a consumer group.
        /// </summary>
        public AsyncApiSchema ClientId { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public BindingType Type => BindingType.Kafka;

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
            writer.WriteRequiredObject(AsyncApiConstants.GroupId, GroupId, (w, h) => h.SerializeV2(w));
            writer.WriteRequiredObject(AsyncApiConstants.ClientId, ClientId, (w, h) => h.SerializeV2(w));
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
