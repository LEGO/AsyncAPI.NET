// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.IO;
    using System.Text;
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Converts contents on JSON/YAML string into AsyncApiDocument instance.
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