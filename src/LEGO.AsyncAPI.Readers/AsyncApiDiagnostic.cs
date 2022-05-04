// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            this.Errors = new List<Exception>();
            if (e is not null)
            {
                this.Errors.Add(e);
            }
        }

        /// <summary>
        /// Returns true if Error value is not null.
        /// </summary>
        public bool HasError => this.Errors.Any();

        /// <summary>
        /// Gets an error.
        /// </summary>
        public List<Exception> Errors { get; }
    }
}
