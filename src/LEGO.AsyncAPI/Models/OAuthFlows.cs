﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Allows configuration of the supported OAuth Flows.
    /// </summary>
    public class OAuthFlows : IExtensible
    {
        /// <summary>
        /// Configuration for the OAuth Implicit flow.
        /// </summary>
        public OAuthFlow Implicit { get; set; }

        /// <summary>
        /// Configuration for the OAuth Resource Owner Protected Credentials flow.
        /// </summary>
        public OAuthFlow Password { get; set; }

        /// <summary>
        /// Configuration for the OAuth Client Credentials flow.
        /// </summary>
        public OAuthFlow ClientCredentials { get; set; }

        /// <summary>
        /// Configuration for the OAuth Authorization Code flow.
        /// </summary>
        public OAuthFlow AuthorizationCode { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; } = new Dictionary<string, IAny>();
    }
}