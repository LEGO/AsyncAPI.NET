// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiOAuthFlow : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. The authorization URL to be used for this flow.
        /// Applies to implicit and authorizationCode OAuthFlow.
        /// </summary>
        public Uri AuthorizationUrl { get; set; }

        /// <summary>
        /// REQUIRED. The token URL to be used for this flow.
        /// Applies to password, clientCredentials, and authorizationCode OAuthFlow.
        /// </summary>
        public Uri TokenUrl { get; set; }

        /// <summary>
        /// The URL to be used for obtaining refresh tokens.
        /// </summary>
        public Uri RefreshUrl { get; set; }

        /// <summary>
        /// REQUIRED. A map between the scope name and a short description for it.
        /// </summary>
        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOAuthFlow"/> to Async Api v2.4
        /// </summary>
        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            // authorizationUrl
            writer.WriteProperty(AsyncApiConstants.AuthorizationUrl, this.AuthorizationUrl?.ToString());

            // tokenUrl
            writer.WriteProperty(AsyncApiConstants.TokenUrl, this.TokenUrl?.ToString());

            // refreshUrl
            writer.WriteProperty(AsyncApiConstants.RefreshUrl, this.RefreshUrl?.ToString());

            // scopes
            writer.WriteRequiredMap(AsyncApiConstants.Scopes, this.Scopes, (w, s) => w.WriteValue(s));

            // extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }
    }
}