// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Interface;

    /// <summary>
    /// Internal class, converts contents on JSON string stream to AsyncApiDocument instance.
    /// </summary>
    internal class AsyncApiJsonReader : IAsyncApiReader<Stream>
    {
        /// <summary>
        /// Converts AsyncApi JSON definition into an instance of AsyncApiDocument.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi JSON definition to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
        {
            diagnostic = new AsyncApiDiagnostic();

            AsyncApiDocument document;

            var internalAsyncApiReader = new InternalJsonAsyncApiReader<AsyncApiDocument>();

            try
            {
                document = internalAsyncApiReader.Read(input);
            }
            catch (Exception e)
            {
                diagnostic = AsyncApiDiagnostic.OnError(e);
                return new AsyncApiDocument();
            }

            return document;
        }
    }
}