// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// The object provides metadata about the API. The metadata can be used by the clients if needed.
    /// </summary>
    public class Info : IAsyncApiExtensible
    {
        public Info(string title, string version)
        {
            this.Title = title;
            this.Version = version;
        }

        /// <summary>
        /// Gets or sets rEQUIRED. The title of the application.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. Provides the version of the application API (not to be confused with the specification version).
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a short description of the application. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a URL to the Terms of Service for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri TermsOfService { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the exposed API.
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the license information for the exposed API.
        /// </summary>
        public License License { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();
    }
}