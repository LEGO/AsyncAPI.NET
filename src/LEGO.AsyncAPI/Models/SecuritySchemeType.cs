// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    /// <summary>
    /// The type of the security scheme.
    /// </summary>
    public enum SecuritySchemeType
    {
        /// <summary>
        /// None.
        /// </summary>
        [Display("None")]None = 0,

        /// <summary>
        /// Username & Password.
        /// </summary>
        [Display("userPassword")] UserPassword,

        /// <summary>
        /// Api Key.
        /// </summary>
        [Display("apiKey")] ApiKey,

        /// <summary>
        /// Certificate.
        /// </summary>
        [Display("X509")] X509,

        /// <summary>
        /// Symmetric Encryption.
        /// </summary>
        [Display("symmetricEncryption")]SymmetricEncryption,

        /// <summary>
        /// Asymmetric Encryption.
        /// </summary>
        [Display("asymmetricEncryption")]AsymmetricEncryption,

        /// <summary>
        /// Api Key.
        /// </summary>
        [Display("httpApiKey")]HttpApiKey,

        /// <summary>
        /// Basic or Bearer token authorization header.
        /// </summary>
        [Display("http")]Http,

        /// <summary>
        /// OAuth2.
        /// </summary>
        [Display("oauth2")]OAuth2,

        /// <summary>
        /// OIDC.
        /// </summary>
        [Display("openIdConnect")]OpenIdConnect,

        /// <summary>
        /// Plain.
        /// </summary>
        [Display("plain")]Plain,

        /// <summary>
        /// Sha256.
        /// </summary>
        [Display("scramSha256")]ScramSha256,

        /// <summary>
        /// Sha512.
        /// </summary>
        [Display("scramSha512")]ScramSha512,

        /// <summary>
        /// GssApi.
        /// </summary>
        [Display("gssapi")]Gssapi,
    }
}