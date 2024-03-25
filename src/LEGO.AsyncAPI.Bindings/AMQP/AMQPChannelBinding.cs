// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for AMQP channel settings.
    /// </summary>
    public class AMQPChannelBinding : ChannelBinding<AMQPChannelBinding>
    {
        /// <summary>
        /// Defines what type of channel is it. Can be either queue or routingKey.
        /// </summary>
        public ChannelType Is { get; set; }

        /// <summary>
        /// When is=routingKey, this object defines the exchange properties.
        /// </summary>
        public Exchange Exchange { get; set; }

        /// <summary>
        /// When is=queue, this object defines the queue properties.
        /// </summary>
        public Queue Queue { get; set; }

        public override string BindingKey => "amqp";

        protected override FixedFieldMap<AMQPChannelBinding> FixedFieldMap => new ()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "is", (a, n) => { a.Is = n.GetScalarValue().GetEnumFromDisplayName<ChannelType>(); } },
            { "exchange", (a, n) => { a.Exchange = n.ParseMap(ExchangeFixedFields); } },
            { "queue", (a, n) => { a.Queue = n.ParseMap(QueueFixedFields); } },
        };

        private static FixedFieldMap<Exchange> ExchangeFixedFields = new ()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "durable", (a, n) => { a.Durable = n.GetBooleanValue(); } },
            { "type", (a, n) => { a.Type = n.GetScalarValue().GetEnumFromDisplayName<ExchangeType>(); } },
            { "autoDelete", (a, n) => { a.AutoDelete = n.GetBooleanValue(); } },
            { "vhost", (a, n) => { a.Vhost = n.GetScalarValue(); } },
        };

        private static FixedFieldMap<Queue> QueueFixedFields = new()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "durable", (a, n) => { a.Durable = n.GetBooleanValue(); } },
            { "exclusive", (a, n) => { a.Exclusive = n.GetBooleanValue(); } },
            { "autoDelete", (a, n) => { a.AutoDelete = n.GetBooleanValue(); } },
            { "vhost", (a, n) => { a.Vhost = n.GetScalarValue(); } },
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
            writer.WriteRequiredProperty("is", this.Is.GetDisplayName());
            switch (this.Is)
            {
                case ChannelType.RoutingKey:
                    writer.WriteOptionalObject("exchange", this.Exchange, (w, t) => t.Serialize(w));
                    break;
                case ChannelType.Queue:
                    writer.WriteOptionalObject("queue", this.Queue, (w, t) => t.Serialize(w));
                    break;
            }

            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }
    }
}
