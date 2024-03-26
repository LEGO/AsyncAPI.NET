// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.MQTT
{
    using System;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for MQTT operations.
    /// </summary>
    public class MQTTOperationBinding : OperationBinding<MQTTOperationBinding>
    {
        /// <summary>
        /// Defines the Quality of Service (QoS) levels for the message flow between client and server.
        /// Its value MUST be either 0 (At most once delivery), 1 (At least once delivery), or 2 (Exactly once delivery).
        /// </summary>
        public int QoS { get; set; }

        /// <summary>
        /// Whether the broker should retain the message or not.
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// Interval in seconds or a Schema Object containing the definition of the lifetime of the message.
        /// </summary>
        public int? MessageExpiryInterval { get; set; }

        public override string BindingKey => "mqtt";

        protected override FixedFieldMap<MQTTOperationBinding> FixedFieldMap => new()
        {
            { "qos", (a, n) => { a.QoS = n.GetIntegerValue(); } },
            { "retain", (a, n) => { a.Retain = n.GetBooleanValue(); } },
            { "messageExpiryInterval", (a, n) => { a.MessageExpiryInterval = n.GetIntegerValueOrDefault(); } },
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
            writer.WriteRequiredProperty("qos", this.QoS);
            writer.WriteRequiredProperty("retain", this.Retain);
            writer.WriteOptionalProperty("messageExpiryInterval", this.MessageExpiryInterval);
            writer.WriteOptionalProperty("bindingVersion", this.BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}
