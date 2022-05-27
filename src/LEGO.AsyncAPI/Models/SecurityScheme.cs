﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Defines a security scheme that can be used by the operations.
    /// </summary>
    public class SecurityScheme : IAsyncApiExtensible, IReferenceable
    {
        public SecurityScheme(SecuritySchemeType type, string name, string @in, string scheme, OAuthFlows flows, Uri openIdConnectUrl)
        {
            this.Type = type;
            this.Name = name;
            this.In = @in;
            this.Scheme = scheme;
            this.Flows = flows;
            this.OpenIdConnectUrl = openIdConnectUrl;
        }

        /// <summary>
        /// Gets or sets rEQUIRED. The type of the security scheme. Valid values are "apiKey", "http", "oauth2", "openIdConnect".
        /// </summary>
        public SecuritySchemeType Type { get; set; }

        /// <summary>
        /// Gets or sets a short description for security scheme. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        /// <remarks>
        /// Applies to type: Any.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. The name of the header, query or cookie parameter to be used.
        /// </summary>
        /// <remarks>
        /// Applies to type: <see cref="SecuritySchemeType.HttpApiKey"/>.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. The location of the API key. Valid values are "user" and "password" for apiKey and "query", "header" or "cookie" for httpApiKey.
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. The name of the HTTP Authorization scheme to be used in the Authorization header as defined in RFC7235.
        /// </summary>
        /// <remarks>
        /// Applies to type: <see cref="SecuritySchemeType.Http"/>.
        /// </remarks>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets a hint to the client to identify how the bearer token is formatted.
        /// Bearer tokens are usually generated by an authorization server,
        /// so this information is primarily for documentation purposes.
        /// </summary>
        /// <remarks>
        /// Applies to type: <see cref="SecuritySchemeType.Http"/> ("bearer").
        /// </remarks>
        public string BearerFormat { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. An object containing configuration information for the flow types supported.
        /// </summary>
        /// <remarks>
        /// Applies to type: <see cref="SecuritySchemeType.Oauth2"/>.
        /// </remarks>
        public OAuthFlows Flows { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        /// <remarks>
        /// Applies to type: <see cref="SecuritySchemeType.Oauth2"/>.
        /// </remarks>
        public Uri OpenIdConnectUrl { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }
    }
}