// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Exceptions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Defines an exception indicating AsyncApi Reader encountered an unsupported specification version while reading.
    /// </summary>
    [Serializable]
    public class AsyncApiUnsupportedSpecVersionException : Exception
    {
        const string MessagePattern = "AsyncApi specification version '{0}' is not supported.";

        /// <summary>
        /// Initializes the <see cref="AsyncApiUnsupportedSpecVersionException"/> class with a specification version.
        /// </summary>
        /// <param name="specificationVersion">Version that caused this exception to be thrown.</param>
        public AsyncApiUnsupportedSpecVersionException(string specificationVersion)
            : base(string.Format(CultureInfo.InvariantCulture, MessagePattern, specificationVersion))
        {
            this.SpecificationVersion = specificationVersion;
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiUnsupportedSpecVersionException"/> class with a specification version and
        /// inner exception.
        /// </summary>
        /// <param name="specificationVersion">Version that caused this exception to be thrown.</param>
        /// <param name="innerException">Inner exception that caused this exception to be thrown.</param>
        public AsyncApiUnsupportedSpecVersionException(string specificationVersion, Exception innerException)
            : base(string.Format(CultureInfo.InvariantCulture, MessagePattern, specificationVersion), innerException)
        {
            this.SpecificationVersion = specificationVersion;
        }

        /// <summary>
        /// The unsupported specification version.
        /// </summary>
        public string SpecificationVersion { get; }
    }
}