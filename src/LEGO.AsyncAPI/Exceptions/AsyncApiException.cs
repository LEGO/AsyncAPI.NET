// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Exceptions
{
    using System;

    public class AsyncApiException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AsyncApiException"/> class with default values.
        /// </summary>
        public AsyncApiException()
            : this("An error occurred while processing the AsyncApi document.")
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AsyncApiException"/> class with an error message.
        /// </summary>
        /// <param name="message">The plain text error message for this exception.</param>
        public AsyncApiException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AsyncApiException"/> class with an error message and an inner exception.
        /// </summary>
        /// <param name="message">The plain text error message for this exception.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception to be thrown.</param>
        public AsyncApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets or sets the reference pointer.  This is a fragment identifier used to point to where the error occurred in the document.
        /// If the document has been parsed as JSON/YAML then the identifier will be a
        /// JSON Pointer as per https://tools.ietf.org/html/rfc6901
        /// If the document fails to parse as JSON/YAML then the fragment will be based on
        /// a text/plain pointer as defined in https://tools.ietf.org/html/rfc5147
        /// Currently only line= is provided because using char= causes tests to break due to CR/LF and LF differences.
        /// </summary>
        public string Pointer { get; set; }
    }
}
