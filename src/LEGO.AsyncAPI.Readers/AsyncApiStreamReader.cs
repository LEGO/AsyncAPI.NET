using System;
using System.IO;
using System.Threading.Tasks;
using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncApi.Readers.Interface;

namespace LEGO.AsyncApi.Readers
{
    /// <summary>
    /// Service class for converting streams into AsyncApiDocument instances
    /// </summary>
    public class AsyncApiStreamReader : IAsyncApiReader<Stream, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings _settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiStreamReader(AsyncApiReaderSettings settings = null)
        {
            _settings = settings ?? new AsyncApiReaderSettings();

            if((_settings.ReferenceResolution == ReferenceResolutionSetting.ResolveAllReferences || _settings.LoadExternalRefs)
                && _settings.BaseUrl == null)
            {
                throw new ArgumentException("BaseUrl must be provided to resolve external references.");
            }
        }

        /// <summary>
        /// Reads the stream input and parses it into an Open API document.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
        {
            var reader = new StreamReader(input);
            var result = new AsyncApiTextReaderReader(_settings).Read(reader, out diagnostic);
            if (!_settings.LeaveStreamOpen)
            {
                reader.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Reads the stream input and parses it into an Open API document.
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

            return await new AsyncApiTextReaderReader(_settings).ReadAsync(reader);
        }

        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncApi description into an AsyncApi Element.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi description to parse.</param>
        /// <param name="version">Version of the AsyncApi specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public T ReadFragment<T>(Stream input, AsyncApiSpecVersion version, out AsyncApiDiagnostic diagnostic) where T : IAsyncApiReferenceable
        {
            using (var reader = new StreamReader(input))
            {
                return new AsyncApiTextReaderReader(_settings).ReadFragment<T>(reader, version, out diagnostic);
            }
        }
    }
}
