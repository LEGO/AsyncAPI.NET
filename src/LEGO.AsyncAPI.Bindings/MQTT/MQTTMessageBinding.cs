// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.MQTT
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for MQTT messages.
    /// </summary>
    public class MQTTMessageBinding : MessageBinding<MQTTMessageBinding>
    {
        /// <summary>
        /// Indicates the format of the payload.
        /// Either: 0 (zero) for unspecified bytes, or 1 for UTF-8 encoded character data.
        /// </summary>
        public int? PayloadFormatIndicator { get; set; }

        /// <summary>
        /// Correlation Data is used to identify the request the response message is for.
        /// </summary>
        public AsyncApiSchema CorrelationData { get; set; }

        /// <summary>
        /// String describing the content type of the message payload.
        /// This should not conflict with the contentType field of the associated AsyncAPI Message object.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The topic (channel URI) for a response message.
        /// </summary>
        public string ResponseTopic { get; set; }

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty("payloadFormatIndicator", this.PayloadFormatIndicator);
            writer.WriteOptionalObject("correlationData", this.CorrelationData, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalProperty("contentType", this.ContentType);
            writer.WriteOptionalProperty("responseTopic", this.ResponseTopic);
            writer.WriteOptionalProperty("bindingVersion", this.BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }

        public override string BindingKey => "mqtt";

        protected override FixedFieldMap<MQTTMessageBinding> FixedFieldMap => new()
        {
            { "payloadFormatIndicator", (a, n) => { a.PayloadFormatIndicator = n.GetIntegerValueOrDefault(); } },
            { "correlationData", (a, n) => { a.CorrelationData = AsyncApiSchemaDeserializer.LoadSchema(n); } },
            { "contentType", (a, n) => { a.ContentType = n.GetScalarValue(); } },
            { "responseTopic", (a, n) => { a.ResponseTopic = n.GetScalarValue(); } },
        };
    }
}
