// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.IO;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;

    /// <summary>
    /// Service class for converting streams into AsyncApiDocument instances
    /// </summary>
    public class AsyncApiStreamReader : IAsyncApiReader<Stream, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiStreamReader(AsyncApiReaderSettings settings = null)
        {
            this.settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
        {
            var reader = new StreamReader(input);
            var result = new AsyncApiTextReader(this.settings).Read(reader, out diagnostic);
            if (!this.settings.LeaveStreamOpen)
            {
                reader.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi description to parse.</param>
        /// <returns>Instance result containing newly created AsyncApiDocument and diagnostics object from the process</returns>
        public async Task<ReadResult> ReadAsync(Stream input)
        {
            MemoryStream bufferedStream;
            if (input is MemoryStream)
            {
                bufferedStream = (MemoryStream)input;
            }
            else
            {
                // Buffer stream so that AsyncApiTextReaderReader can process it synchronously
                // YamlDocument doesn't support async reading.
                bufferedStream = new MemoryStream();
                await input.CopyToAsync(bufferedStream);
                bufferedStream.Position = 0;
            }

            var reader = new StreamReader(bufferedStream);

            return await new AsyncApiTextReader(this.settings).ReadAsync(reader);
        }

        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncApi description into an AsyncApi Element.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi description to parse.</param>
        /// <param name="version">Version of the AsyncApi specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public T ReadFragment<T>(Stream input, AsyncApiVersion version, out AsyncApiDiagnostic diagnostic)
            where T : IAsyncApiReferenceable
        {
            using (var reader = new StreamReader(input))
            {
                return new AsyncApiTextReader(this.settings).ReadFragment<T>(reader, version, out diagnostic);
            }
        }
    }
}
