// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// OAuth Flows Object.
    /// </summary>
    public class AsyncApiOauthFlows : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Configuration for the OAuth Implicit flow
        /// </summary>
        public AsyncApiOauthFlow Implicit { get; set; }

        /// <summary>
        /// Configuration for the OAuth Resource Owner Password flow.
        /// </summary>
        public AsyncApiOauthFlow Password { get; set; }

        /// <summary>
        /// Configuration for the OAuth Client Credentials flow.
        /// </summary>
        public AsyncApiOauthFlow ClientCredentials { get; set; }

        /// <summary>
        /// Configuration for the OAuth Authorization Code flow.
        /// </summary>
        public AsyncApiOauthFlow AuthorizationCode { get; set; }

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOauthFlows"/> to Open Api v3.0
        /// </summary>
        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            // implicit
            writer.WriteOptionalObject(AsyncApiConstants.Implicit, this.Implicit, (w, o) => o.SerializeV2(w));

            // password
            writer.WriteOptionalObject(AsyncApiConstants.Password, this.Password, (w, o) => o.SerializeV2(w));

            // clientCredentials
            writer.WriteOptionalObject(
                AsyncApiConstants.ClientCredentials,
                this.ClientCredentials,
                (w, o) => o.SerializeV2(w));

            // authorizationCode
            writer.WriteOptionalObject(
                AsyncApiConstants.AuthorizationCode,
                this.AuthorizationCode,
                (w, o) => o.SerializeV2(w));

            // extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }
    }
}