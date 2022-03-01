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
        /// The Error.
        /// </summary>
        public Exception Error { get; }

        public AsyncApiDiagnostic()
        {
        }

        public AsyncApiDiagnostic(Exception e)
        {
            Error = e;
        }
    }
}