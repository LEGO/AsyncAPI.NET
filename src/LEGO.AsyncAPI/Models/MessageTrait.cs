// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a trait that MAY be applied to a Message Object.
    /// </summary>
    public class MessageTrait : IAsyncApiExtensible, IReferenceable
    {
        /// <summary>
        /// Gets or sets schema definition of the application headers. Schema MUST be of type "object".
        /// </summary>
        public Schema Headers { get; set; }

        /// <summary>
        /// Gets or sets definition of the correlation ID used for message tracing or matching.
        /// </summary>
        public CorrelationId CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets a string containing the name of the schema format used to define the message payload.
        /// </summary>
        /// <remarks>
        /// If omitted, implementations should parse the payload as a Schema object.
        /// </remarks>
        public string SchemaFormat { get; set; }

        /// <summary>
        /// Gets or sets the content type to use when encoding/decoding a message's payload.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets a machine-friendly name for the message.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a human-friendly title for the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a short summary of what the message is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets a verbose explanation of the message. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a list of tags for API documentation control. Tags can be used for logical grouping of messages.
        /// </summary>
        public IList<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Gets or sets additional external documentation for this message.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
        /// </summary>
        [JsonConverter(typeof(MessageJsonDictionaryContractBindingConverter))]
        public IDictionary<string, IMessageBinding> Bindings { get; set; } = new Dictionary<string, IMessageBinding>();

        /// <summary>
        /// Gets or sets list of examples.
        /// </summary>
        public IList<MessageExample> Examples { get; set; } = new List<MessageExample>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }
    }
}