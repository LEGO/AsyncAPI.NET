// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.IO;
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Internal class, converts contents on JSON string stream to AsyncApiDocument instance.
    /// </summary>
    internal class AsyncApiJsonReader : IAsyncApiReader<Stream>
    {
        private readonly JsonStreamReader<AsyncApiDocument> jsonStreamReader;

        public AsyncApiJsonReader()
        {
            this.jsonStreamReader = new JsonStreamReader<AsyncApiDocument>();
        }

        /// <summary>
        /// Converts AsyncApi JSON definition into an instance of AsyncApiDocument.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi JSON definition to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
        {
            diagnostic = new AsyncApiDiagnostic(null);
            var document = new AsyncApiDocument();

            try
            {
                document = this.jsonStreamReader.Read(input);
            }
            catch (Exception e)
            {
                diagnostic = new AsyncApiDiagnostic(e);
            }

            return document;
        }
    }
}