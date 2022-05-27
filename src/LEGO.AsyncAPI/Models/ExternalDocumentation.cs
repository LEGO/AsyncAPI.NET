// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    public class ExternalDocumentation : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets a short description of the target documentation. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. The URL for the target documentation. Value MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();
    }
}