// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// An object that specifies an identifier at design time that can used for message tracing and correlation.
    /// </summary>
    public class CorrelationId : IReferenceable, IExtensible
    {
        /// <summary>
        /// An optional description of the identifier. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. A runtime expression that specifies the location of the correlation ID.
        /// </summary>
        public string Location { get; set; }

        /// <inheritdoc/>
        public bool UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public Reference Reference { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();
    }
}