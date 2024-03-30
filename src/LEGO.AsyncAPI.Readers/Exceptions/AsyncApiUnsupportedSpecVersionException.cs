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
        private const string MessagePattern = "AsyncApi specification version '{0}' is not supported.";

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiUnsupportedSpecVersionException"/> class.
        /// </summary>
        /// <param name="specificationVersion">Version that caused this exception to be thrown.</param>
        /// <param name="settings">The settings used for reading and writing.</param>
        public AsyncApiUnsupportedSpecVersionException(string specificationVersion)
            : base(string.Format(CultureInfo.InvariantCulture, MessagePattern, specificationVersion))
        {
            this.SpecificationVersion = specificationVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiUnsupportedSpecVersionException"/> class.
        /// inner exception.
        /// </summary>
        /// <param name="specificationVersion">Version that caused this exception to be thrown.</param>
        /// <param name="settings">The setting used for reading and writing</param>
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