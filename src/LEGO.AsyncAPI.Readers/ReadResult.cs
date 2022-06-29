// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Container object used for returning the result of reading an AsyncApi description.
    /// </summary>
    public class ReadResult
    {
        /// <summary>
        /// The parsed AsyncApiDocument.  Null will be returned if the document could not be parsed.
        /// </summary>
        public AsyncApiDocument AsyncApiDocument { set; get; }
        /// <summary>
        /// AsyncApiDiagnostic contains the Errors reported while parsing
        /// </summary>
        public AsyncApiDiagnostic AsyncApiDiagnostic { set; get; }
    }
}
