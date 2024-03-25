// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Represents a queue configuration.
    /// </summary>
    public class Queue : IAsyncApiElement
    {
        /// <summary>
        /// The name of the queue. It MUST NOT exceed 255 characters long.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the queue should survive broker restarts or not.
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// Whether the queue should be used only by one connection or not.
        /// </summary>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Whether the queue should be deleted when the last consumer unsubscribes.
        /// </summary>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// The virtual host of the queue. Defaults to /.
        /// </summary>
        public string Vhost { get; set; } = "/";

        public void Serialize(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteRequiredProperty(AsyncApiConstants.Name, this.Name);
            writer.WriteRequiredProperty("durable", this.Durable);
            writer.WriteRequiredProperty("exclusive", this.Exclusive);
            writer.WriteRequiredProperty("autoDelete", this.AutoDelete);
            writer.WriteRequiredProperty("vhost", this.Vhost);
            writer.WriteEndObject();
        }
    }
}
