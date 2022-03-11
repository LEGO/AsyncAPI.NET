﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// This is the root document object for the API specification. It combines resource listing and API declaration together into one document.
    /// </summary>
    public class AsyncApiDocument : IExtensible
    {
        /// <summary>
        /// REQUIRED. Specifies the AsyncAPI Specification version being used.
        /// </summary>
        public string Asyncapi { get; set; }

        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// REQUIRED. Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        public Info Info { get; set; }

        /// <summary>
        /// Provides connection details of servers. Field pattern ^[A-Za-z0-9_\-]+$.
        /// </summary>
        public IDictionary<string, Server> Servers { get; set; } = new Dictionary<string, Server>();

        /// <summary>
        /// Default content type to use when encoding/decoding a message's payload.
        /// </summary>
        /// <remarks>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json). This value MUST be used by schema parsers when the contentType property is omitted.
        /// </remarks>
        public string DefaultContentType { get; set; }

        /// <summary>
        /// REQUIRED. The available channels and messages for the API.
        /// </summary>
        public IDictionary<string, Channel> Channels { get; set; } = new Dictionary<string, Channel>();

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        public Components Components { get; set; }

        /// <summary>
        /// A list of tags used by the specification with additional metadata. Each tag name in the list MUST be unique.
        /// </summary>
        public IList<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; } = new Dictionary<string, IAny>();
    }
}
