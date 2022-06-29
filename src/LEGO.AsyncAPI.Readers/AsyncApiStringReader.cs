// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.IO;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;

    /// <summary>
    /// Service class for converting strings into AsyncApiDocument instances
    /// </summary>
    public class AsyncApiStringReader : IAsyncApiReader<string, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;

        /// <summary>
        /// Constructor tha allows reader to use non-default settings
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiStringReader(AsyncApiReaderSettings settings = null)
        {
            this.settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the string input and parses it into an AsyncApi document.
        /// </summary>
        public AsyncApiDocument Read(string input, out AsyncApiDiagnostic diagnostic)
        {
            using (var reader = new StringReader(input))
            {
                return new AsyncApiTextReaderReader(this.settings).Read(reader, out diagnostic);
            }
        }

        /// <summary>
        /// Reads the string input and parses it into an AsyncApi element.
        /// </summary>
        public T ReadFragment<T>(string input, AsyncApiVersion version, out AsyncApiDiagnostic diagnostic)
            where T : IAsyncApiElement
        {
            using (var reader = new StringReader(input))
            {
                return new AsyncApiTextReaderReader(this.settings).ReadFragment<T>(reader, version, out diagnostic);
            }
        }
    }
}
