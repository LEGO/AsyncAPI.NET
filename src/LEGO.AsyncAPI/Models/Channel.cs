// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes the operations available on a single channel.
    /// </summary>
    public class Channel : IReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets an optional description of this channel item. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the servers on which this channel is available, specified as an optional unordered list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// </summary>
        /// <remarks>
        /// If servers is absent or empty then this channel must be available on all servers defined in the Servers Object.
        /// </remarks>
        public IList<string> Servers { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
        /// </summary>
        public Operation Subscribe { get; set; }

        /// <summary>
        /// Gets or sets a definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
        /// </summary>
        public Operation Publish { get; set; }

        /// <summary>
        /// Gets or sets a map of the parameters included in the channel name. It SHOULD be present only when using channels with expressions (as defined by RFC 6570 section 2.2).
        /// </summary>
        public IDictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();

        /// <summary>
        /// Gets or sets a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the channel.
        /// </summary>
        [JsonConverter(typeof(ChannelJsonDictionaryContractBindingConverter))]
        public IDictionary<string, IChannelBinding> Bindings { get; set; } = new Dictionary<string, IChannelBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }
    }
}