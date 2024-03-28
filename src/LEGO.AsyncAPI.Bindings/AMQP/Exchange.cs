// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Represents an exchange configuration.
    /// </summary>
    public class Exchange : IAsyncApiElement
    {
        /// <summary>
        /// The name of the exchange. It MUST NOT exceed 255 characters long.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the exchange. Can be either topic, direct, fanout, default, or headers.
        /// </summary>
        public ExchangeType Type { get; set; }

        /// <summary>
        /// Whether the exchange should survive broker restarts or not.
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// Whether the exchange should be deleted when the last queue is unbound from it.
        /// </summary>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// The virtual host of the exchange. Defaults to /.
        /// </summary>
        public string Vhost { get; set; } = "/";

        public void Serialize(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteRequiredProperty(AsyncApiConstants.Name, this.Name);
            writer.WriteRequiredProperty(AsyncApiConstants.Type, this.Type.GetDisplayName());
            writer.WriteRequiredProperty("durable", this.Durable);
            writer.WriteRequiredProperty("autoDelete", this.AutoDelete);
            writer.WriteRequiredProperty("vhost", this.Vhost);
            writer.WriteEndObject();
        }
    }
}
