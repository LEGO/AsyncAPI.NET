// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for AMQP messages.
    /// </summary>
    public class AMQPMessageBinding : MessageBinding<AMQPMessageBinding>
    {
        /// <summary>
        /// A MIME encoding for the message content.
        /// </summary>
        public string ContentEncoding { get; set; }

        /// <summary>
        /// Application-specific message type.
        /// </summary>
        public string MessageType { get; set; }

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteOptionalProperty("contentEncoding", this.ContentEncoding);
            writer.WriteOptionalProperty("messageType", this.MessageType);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        public override string BindingKey => "amqp";

        protected override FixedFieldMap<AMQPMessageBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "contentEncoding", (a, n) => { a.ContentEncoding = n.GetScalarValue(); } },
            { "messageType", (a, n) => { a.MessageType = n.GetScalarValue(); } },
        };
    }
}
