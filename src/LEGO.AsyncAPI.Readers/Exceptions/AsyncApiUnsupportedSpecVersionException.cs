using System;
using System.Globalization;

namespace LEGO.AsyncAPI.Readers.Exceptions
{
    /// <summary>
    /// Defines an exception indicating AsyncApi Reader encountered an unsupported specification version while reading.
    /// </summary>
    [Serializable]
    public class AsyncApiUnsupportedSpecVersionException : Exception
    {
        const string messagePattern = "AsyncApi specification version '{0}' is not supported.";

        /// <summary>
        /// Initializes the <see cref="AsyncApiUnsupportedSpecVersionException"/> class with a specification version.
        /// </summary>
        /// <param name="specificationVersion">Version that caused this exception to be thrown.</param>
        public AsyncApiUnsupportedSpecVersionException(string specificationVersion)
            : base(string.Format(CultureInfo.InvariantCulture, messagePattern, specificationVersion))
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
            : base(string.Format(CultureInfo.InvariantCulture, messagePattern, specificationVersion), innerException)
        {
            this.SpecificationVersion = specificationVersion;
        }

        /// <summary>
        /// The unsupported specification version.
        /// </summary>
        public string SpecificationVersion { get; }
    }
}