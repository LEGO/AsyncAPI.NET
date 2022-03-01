// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Text;
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Converts contents on JSON/YAML string into AsyncApiDocument instance.
    /// </summary>
    internal class AsyncApiStringReader : IAsyncApiReader<string>
    {
        private IAsyncApiReader<Stream> _jsonAsyncApiReader;

        public AsyncApiStringReader()
        {
            _jsonAsyncApiReader = new AsyncApiJsonReader();
        }

        /// <summary>
        /// Reads the string input and parses it into an AsyncApiDocument.
        /// </summary>
        /// <param name="input">String containing AsyncApi definition to parse. Supports JSON and YAML formats.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(string input, out AsyncApiDiagnostic diagnostic)
        {
            AsyncApiDocument output = new AsyncApiDocument();

            try
            {
                output = _jsonAsyncApiReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(input)), out diagnostic);
            }
            catch (Exception e)
            {
                diagnostic = new AsyncApiDiagnostic(e);
                return output;
            }

            return output;
        }
    }
}