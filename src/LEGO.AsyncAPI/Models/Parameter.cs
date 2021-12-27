// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// Describes a parameter included in a channel name.
    /// </summary>
    public class Parameter : IReferenceable, IExtensible
    {
        /// <summary>
        /// A verbose explanation of the parameter. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Definition of the parameter.
        /// </summary>
        public Schema Schema { get; set; }

        /// <summary>
        /// A runtime expression that specifies the location of the parameter value.
        /// </summary>
        public string Location { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public bool UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public Reference Reference { get; set; }
    }
}