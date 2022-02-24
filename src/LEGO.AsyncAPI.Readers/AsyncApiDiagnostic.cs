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
        public Exception Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static AsyncApiDiagnostic OnError(Exception ex)
        {
            return new () { Error = ex };
        }
    }
}