// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;

    /// <summary>
    /// Object containing errors that happened during AsyncApi parsing.
    /// </summary>
    public class AsyncApiDiagnostic
    {
        /// <summary>
        /// Initializes new instance of AsyncApiDiagnostic class.
        /// </summary>
        /// <param name="e"></param>
        public AsyncApiDiagnostic(Exception e = null)
        {
            this.Error = e;
        }

        /// <summary>
        /// Returns true if Error value is not null.
        /// </summary>
        public bool HasError => this.Error != null;

        /// <summary>
        /// Gets an error.
        /// </summary>
        public Exception Error { get; }
    }
}