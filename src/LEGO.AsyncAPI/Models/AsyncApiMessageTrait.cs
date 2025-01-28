// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Describes a trait that MAY be applied to a Message Object.
    /// </summary>
    public class AsyncApiMessageTrait : IAsyncApiExtensible, IAsyncApiSerializable
    {
        /// <summary>
        /// Unique string used to identify the message. The id MUST be unique among all messages described in the API.
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// schema definition of the application headers. Schema MUST be of type "object".
        /// </summary>
        public virtual AsyncApiJsonSchema Headers { get; set; }

        /// <summary>
        /// definition of the correlation ID used for message tracing or matching.
        /// </summary>
        public virtual AsyncApiCorrelationId CorrelationId { get; set; }

        /// <summary>
        /// a string containing the name of the schema format used to define the message payload.
        /// </summary>
        /// <remarks>
        /// If omitted, implementations should parse the payload as a Schema object.
        /// </remarks>
        public virtual string SchemaFormat { get; set; }

        /// <summary>
        /// the content type to use when encoding/decoding a message's payload.
        /// </summary>
        public virtual string ContentType { get; set; }

        /// <summary>
        /// a machine-friendly name for the message.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// a human-friendly title for the message.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// a short summary of what the message is about.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// a verbose explanation of the message. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// a list of tags for API documentation control. Tags can be used for logical grouping of messages.
        /// </summary>
        public virtual IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// additional external documentation for this message.
        /// </summary>
        public virtual AsyncApiExternalDocumentation ExternalDocs { get; set; }
        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
        /// </summary>
        public virtual AsyncApiBindings<IMessageBinding> Bindings { get; set; } = new AsyncApiBindings<IMessageBinding>();

        /// <summary>
        /// list of examples.
        /// </summary>
        public virtual IList<AsyncApiMessageExample> Examples { get; set; } = new List<AsyncApiMessageExample>();

        /// <inheritdoc/>
        public virtual IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public virtual void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty(AsyncApiConstants.MessageId, this.MessageId);
            writer.WriteOptionalObject(AsyncApiConstants.Headers, this.Headers, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.CorrelationId, this.CorrelationId, (w, c) => c.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.SchemaFormat, this.SchemaFormat);
            writer.WriteOptionalProperty(AsyncApiConstants.ContentType, this.ContentType);
            writer.WriteOptionalProperty(AsyncApiConstants.Name, this.Name);
            writer.WriteOptionalProperty(AsyncApiConstants.Title, this.Title);
            writer.WriteOptionalProperty(AsyncApiConstants.Summary, this.Summary);
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.Bindings, this.Bindings, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalCollection(AsyncApiConstants.Examples, this.Examples, (w, e) => e.SerializeV2(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}