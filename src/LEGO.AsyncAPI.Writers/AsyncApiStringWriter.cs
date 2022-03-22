// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Converts contents AsyncApiDocument object to JSON string.
    /// </summary>
    public class AsyncApiStringWriter : IAsyncApiWriter
    {
        private readonly JsonStringWriter<AsyncApiDocument> jsonStreamWriter;

        public AsyncApiStringWriter()
        {
            this.jsonStreamWriter = new JsonStringWriter<AsyncApiDocument>();
        }

        public string Write(AsyncApiDocument document, out AsyncApiDiagnostic diagnostic)
        {
            diagnostic = new AsyncApiDiagnostic();
            
            try
            {
                return this.jsonStreamWriter.Write(document);
            }
            catch (Exception e)
            {
                diagnostic = new AsyncApiDiagnostic(e);
                return string.Empty;
            }
        }
    }
}