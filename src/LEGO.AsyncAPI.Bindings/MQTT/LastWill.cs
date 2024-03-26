// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.MQTT
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System;

    public class LastWill : IAsyncApiElement
    {
        /// <summary>
        /// The topic where the Last Will and Testament message will be sent.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Defines how hard the broker/client will try to ensure that
        /// the Last Will and Testament message is received.
        /// Its value MUST be either 0, 1 or 2.
        /// </summary>
        public uint? QoS { get; set; }

        /// <summary>
        /// Last Will message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Whether the broker should retain the Last Will and Testament message or not.
        /// </summary>
        public bool Retain { get; set; }

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty("topic", this.Topic);
            writer.WriteOptionalProperty("qos", (int?)this.QoS);
            writer.WriteOptionalProperty("message", this.Message);
            writer.WriteRequiredProperty("retain", this.Retain);
            writer.WriteEndObject();
        }
    }
}