// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using LEGO.AsyncAPI.Exceptions;

    public class AsyncApiWriterException : AsyncApiException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AsyncApiWriterException"/> class with default values.
        /// </summary>
        public AsyncApiWriterException()
            : this("An error occurred while writing the AsyncApi document.")
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AsyncApiWriterException"/> class with an error message.
        /// </summary>
        /// <param name="message">The plain text error message for this exception.</param>
        public AsyncApiWriterException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AsyncApiWriterException"/> class with an error message and an inner exception.
        /// </summary>
        /// <param name="message">The plain text error message for this exception.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception to be thrown.</param>
        public AsyncApiWriterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
