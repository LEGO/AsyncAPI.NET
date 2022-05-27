// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Allows adding meta data to a single tag.
    /// </summary>
    public class Tag : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets rEQUIRED. The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a short description for the tag. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets additional external documentation for this tag.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();
    }
}