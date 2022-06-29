// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using SharpYaml;
    using SharpYaml.Serialization;

    /// <summary>
    /// Service class for converting contents of TextReader into AsyncApiDocument instances
    /// </summary>
    public class AsyncApiTextReaderReader : IAsyncApiReader<TextReader, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiTextReaderReader(AsyncApiReaderSettings settings = null)
        {
            this.settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public AsyncApiDocument Read(TextReader input, out AsyncApiDiagnostic diagnostic)
        {
            YamlDocument yamlDocument;

            // Parse the YAML/JSON text in the TextReader into the YamlDocument
            try
            {
                yamlDocument = LoadYamlDocument(input);
            }
            catch (YamlException ex)
            {
                diagnostic = new AsyncApiDiagnostic();
                diagnostic.Errors.Add(new AsyncApiError($"#line={ex.Start.Line}", ex.Message));
                return new AsyncApiDocument();
            }

            return new AsyncApiYamlDocumentReader(this.settings).Read(yamlDocument, out diagnostic);
        }

        /// <summary>
        /// Reads the content of the TextReader.  If there are references to external documents then they will be read asynchronously.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <returns>A ReadResult instance that contains the resulting AsyncApiDocument and a diagnostics instance.</returns>
        public async Task<ReadResult> ReadAsync(TextReader input)
        {
            YamlDocument yamlDocument;

            // Parse the YAML/JSON text in the TextReader into the YamlDocument
            try
            {
                yamlDocument = LoadYamlDocument(input);
            }
            catch (YamlException ex)
            {
                var diagnostic = new AsyncApiDiagnostic();
                diagnostic.Errors.Add(new AsyncApiError($"#line={ex.Start.Line}", ex.Message));
                return new ReadResult
                {
                    AsyncApiDocument = null,
                    AsyncApiDiagnostic = diagnostic,
                };
            }

            return await new AsyncApiYamlDocumentReader(this.settings).ReadAsync(yamlDocument);
        }


        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncApi description into an AsyncApi Element.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="version">Version of the AsyncApi specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public T ReadFragment<T>(TextReader input, AsyncApiVersion version, out AsyncApiDiagnostic diagnostic)
            where T : IAsyncApiElement
        {
            YamlDocument yamlDocument;

            // Parse the YAML/JSON
            try
            {
                yamlDocument = LoadYamlDocument(input);
            }
            catch (YamlException ex)
            {
                diagnostic = new AsyncApiDiagnostic();
                diagnostic.Errors.Add(new AsyncApiError($"#line={ex.Start.Line}", ex.Message));
                return default(T);
            }

            return new AsyncApiYamlDocumentReader(this.settings).ReadFragment<T>(yamlDocument, version,
                out diagnostic);
        }

        /// <summary>
        /// Helper method to turn streams into YamlDocument
        /// </summary>
        /// <param name="input">Stream containing YAML formatted text</param>
        /// <returns>Instance of a YamlDocument</returns>
        static YamlDocument LoadYamlDocument(TextReader input)
        {
            var yamlStream = new YamlStream();
            yamlStream.Load(input);
            return yamlStream.Documents.First();
        }
    }
}
