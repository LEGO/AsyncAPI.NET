// Copyright (c) The LEGO Group. All rights reserved.

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
        public string Description { get; set; }

        /// <summary>
        /// The servers on which this channel is available, specified as an optional unordered list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// </summary>
        /// <remarks>
        /// If servers is absent or empty then this channel must be available on all servers defined in the Servers Object.
        /// </remarks>
        public IList<string> Servers { get; set; } = new List<string>();

        /// <summary>
        /// A definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
        /// </summary>
        public Operation Subscribe { get; set; }

        /// <summary>
        /// A definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
        /// </summary>
        public Operation Publish { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public bool UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public Reference Reference { get; set; }
    }
}