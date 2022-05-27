// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Configuration details for a supported OAuth Flow.
    /// </summary>
    public class OAuthFlow : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets REQUIRED. The authorization URL to be used for this flow. This MUST be in the form of a URL.
        /// </summary>
        public Uri AuthorizationUrl { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. The token URL to be used for this flow. This MUST be in the form of a URL.
        /// </summary>
        public Uri TokenUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to be used for obtaining refresh tokens. This MUST be in the form of a URL.
        /// </summary>
        public Uri RefreshUrl { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. The available scopes for the OAuth2 security scheme. A map between the scope name and a short description for it.
        /// </summary>
        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();
    }
}