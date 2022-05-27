// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Describes a parameter included in a channel name.
    /// </summary>
    public class Parameter : IReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets a verbose explanation of the parameter. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets definition of the parameter.
        /// </summary>
        public Schema Schema { get; set; }

        /// <summary>
        /// Gets or sets a runtime expression that specifies the location of the parameter value.
        /// </summary>
        public string Location { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }
    }
}