// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Allows adding meta data to a single tag.
    /// </summary>
    public class AsyncApiTag : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets REQUIRED. The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a short description for the tag. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets additional external documentation for this tag.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();
    }
}