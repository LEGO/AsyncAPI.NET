﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a publish or a subscribe operation. This provides a place to document how and why messages are sent and received.
    /// </summary>
    public class Operation : IExtensible
    {
        /// <summary>
        /// Unique string used to identify the operation.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        public IList<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
        /// </summary>
        [JsonConverter(typeof(OperationBindingConverter))]
        public IDictionary<string, IOperationBinding> Bindings { get; set; }

        /// <summary>
        /// A list of traits to apply to the operation object.
        /// </summary>
        public IList<OperationTrait> Traits { get; set; } = new List<OperationTrait>();

        /// <summary>
        /// A definition of the message that will be published or received on this channel.
        /// </summary>
        /// <remarks>
        /// `oneOf` is allowed here to specify multiple messages, however, a message MUST be valid only against one of the referenced message objects.
        /// </remarks>
        public Message Message { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; } = new Dictionary<string, IAny>();
    }
}