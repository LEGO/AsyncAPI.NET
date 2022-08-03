// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.MessageBindings
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;


    public enum MessageBindingType
    {
        [Display("kafka")]
        Kafka,
        [Display("http")]
        Http,
        [Display("amqp")]
        Ampq
            
    }
    public class KafkaMessageBinding : IMessageBinding
    {
        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteRequiredObject(AsyncApiConstants.Key, this.Key, (w, h) => h.SerializeV2(w));
            writer.WriteProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);

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

        public AsyncApiSchema Key { get; set; }

        public string BindingVersion { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public MessageBindingType Type => MessageBindingType.Kafka;
    }
}