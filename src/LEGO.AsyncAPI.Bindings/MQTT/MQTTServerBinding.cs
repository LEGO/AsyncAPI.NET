// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.MQTT
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for MQTT channel settings.
    /// </summary>
    public class MQTTServerBinding : ServerBinding<MQTTServerBinding>
    {
        /// <summary>
        /// The client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Whether to create a persistent connection or not.
        /// When false, the connection will be persistent.
        /// This is called clean start in MQTTv5.
        /// </summary>
        public bool? CleanSession { get; set; }

        /// <summary>
        /// Last Will and Testament configuration.
        /// </summary>
        public LastWill LastWill { get; set; }

        /// <summary>
        /// Interval in seconds of the longest period of time
        /// the broker and the client can endure without sending a message.
        /// </summary>
        public int? KeepAlive { get; set; }

        /// <summary>
        /// Interval in seconds the broker maintains a session
        /// for a disconnected client until this interval expires.
        /// </summary>
        public int? SessionExpiryInterval { get; set; }

        /// <summary>
        /// Number of bytes representing the maximum packet size
        /// the client is willing to accept.
        /// </summary>
        public int? MaximumPacketSize { get; set; }

        public override string BindingKey => "mqtt";

        protected override FixedFieldMap<MQTTServerBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "clientId", (a, n) => { a.ClientId = n.GetScalarValue(); } },
            { "cleanSession", (a, n) => { a.CleanSession = n.GetBooleanValueOrDefault(); } },
            { "lastWill", (a, n) => { a.LastWill = n.ParseMap(LastWillFixedFields); } },
            { "keepAlive", (a, n) => { a.KeepAlive = n.GetIntegerValueOrDefault(); } },
            { "sessionExpiryInterval", (a, n) => { a.SessionExpiryInterval = n.GetIntegerValueOrDefault(); } },
            { "maximumPacketSize", (a, n) => { a.MaximumPacketSize = n.GetIntegerValueOrDefault(); } },
        };

        private static FixedFieldMap<LastWill> LastWillFixedFields = new()
        {
            { "topic", (a, n) => { a.Topic = n.GetScalarValue(); } },
            { "qos", (a, n) => { a.QoS = (uint?)n.GetIntegerValueOrDefault(); } },
            { "message", (a, n) => { a.Message = n.GetScalarValue(); } },
            { "retain", (a, n) => { a.Retain = n.GetBooleanValue(); } },
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
            writer.WriteRequiredProperty("clientId", this.ClientId);
            writer.WriteOptionalProperty("cleanSession", this.CleanSession);
            writer.WriteOptionalObject("lastWill", this.LastWill, (w, l) => l.Serialize(w));
            writer.WriteOptionalProperty("keepAlive", this.KeepAlive);
            writer.WriteOptionalProperty("sessionExpiryInterval", this.SessionExpiryInterval);
            writer.WriteOptionalProperty("maximumPacketSize", this.MaximumPacketSize);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}
