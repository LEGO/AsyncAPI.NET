// Copyright (c) The LEGO Group. All rights reserved.

using System.Collections.Immutable;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models.ChannelBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// Describes the operations available on a single channel.
    /// </summary>
    public class Channel : IReferenceable, IExtensible
    {
        /// <summary>
        /// An optional description of this channel item. CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The servers on which this channel is available, specified as an optional unordered list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// </summary>
        /// <remarks>
        /// If servers is absent or empty then this channel must be available on all servers defined in the Servers Object.
        /// </remarks>
        [JsonProperty("servers")]
        public IList<string> Servers { get; set; }

        /// <summary>
        /// A definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
        /// </summary>
        [JsonProperty("subscribe")]
        public Operation Subscribe { get; set; }

        /// <summary>
        /// A definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
        /// </summary>
        [JsonProperty("publish")]
        public Operation Publish { get; set; }

        /// <summary>
        /// A map of the parameters included in the channel name. It SHOULD be present only when using channels with expressions (as defined by RFC 6570 section 2.2).
        /// </summary>
        [JsonProperty("parameters")]
        public IDictionary<string, Parameter> Parameters { get; set; }

        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the channel.
        /// </summary>
        [JsonProperty("bindings")]
        [JsonConverter(typeof(ChannelBindingConverter))]
        public IDictionary<string, IChannelBinding> Bindings { get; set; }

        /// <inheritdoc/>
        [JsonExtensionData]
        public IDictionary<string, JToken> Extensions { get; set; }

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public Reference Reference { get; set; }

    }
}