// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// The type of the security scheme.
    /// </summary>
    public enum SecuritySchemeType
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Username & Password.
        /// </summary>
        UserPassword,

        /// <summary>
        /// Api Key.
        /// </summary>
        ApiKey,

        /// <summary>
        /// Certificate.
        /// </summary>
        X509,

        /// <summary>
        /// Symmetric Encryption.
        /// </summary>
        SymmetricEncryption,

        /// <summary>
        /// Asymmetric Encryption.
        /// </summary>
        AsymmetricEncryption,

        /// <summary>
        /// Api Key.
        /// </summary>
        HttpApiKey,

        /// <summary>
        /// Basic or Bearer token authorization header.
        /// </summary>
        Http,

        /// <summary>
        /// OAuth2.
        /// </summary>
        OAuth2,

        /// <summary>
        /// OIDC.
        /// </summary>
        OpenIdConnect,

        /// <summary>
        /// Plain.
        /// </summary>
        Plain,

        /// <summary>
        /// Sha256.
        /// </summary>
        ScramSha256,

        /// <summary>
        /// Sha512.
        /// </summary>
        ScramSha512,

        /// <summary>
        /// GssApi.
        /// </summary>
        Gssapi,
    }
}