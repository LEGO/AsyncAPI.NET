// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using YamlDotNet.RepresentationModel;

    /// <summary>
    /// Service class for converting contents of TextReader into AsyncApiDocument instances.
    /// </summary>
    public class AsyncApiTextReader : IAsyncApiReader<TextReader, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiTextReader(AsyncApiReaderSettings settings = null)
        {
            this.settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(TextReader input, out AsyncApiDiagnostic diagnostic)
        {
            JsonNode jsonNode;

            // Parse the YAML/JSON text in the TextReader into the YamlDocument
            try
            {
                jsonNode = LoadYamlDocument(input);
            }
            catch (JsonException ex)
            {
                diagnostic = new AsyncApiDiagnostic();
                diagnostic.Errors.Add(new AsyncApiError($"#line={ex.LineNumber}", ex.Message));
                return new AsyncApiDocument();
            }

            return new AsyncApiJsonDocumentReader(this.settings).Read(jsonNode, out diagnostic);
        }

        /// <summary>
        /// Reads the content of the TextReader.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <returns>A ReadResult instance that contains the resulting AsyncApiDocument and a diagnostics instance.</returns>
        public async Task<ReadResult> ReadAsync(TextReader input)
        {
            JsonNode jsonNode;

            // Parse the YAML/JSON text in the TextReader into the YamlDocument
            try
            {
                jsonNode = LoadYamlDocument(input);
            }
            catch (JsonException ex)
            {
                var diagnostic = new AsyncApiDiagnostic();
                diagnostic.Errors.Add(new AsyncApiError($"#line={ex.LineNumber}", ex.Message));
                return new ReadResult
                {
                    AsyncApiDocument = null,
                    AsyncApiDiagnostic = diagnostic,
                };
            }

            return await new AsyncApiJsonDocumentReader(this.settings).ReadAsync(jsonNode);
        }

        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncApi description into an AsyncApi Element.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="version">Version of the AsyncApi specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public T ReadFragment<T>(TextReader input, AsyncApiVersion version, out AsyncApiDiagnostic diagnostic)
            where T : IAsyncApiElement
        {
            JsonNode jsonNode;

            // Parse the YAML/JSON
            try
            {
                jsonNode = LoadYamlDocument(input);
            }
            catch (JsonException ex)
            {
                diagnostic = new AsyncApiDiagnostic();
                diagnostic.Errors.Add(new AsyncApiError($"#line={ex.LineNumber}", ex.Message));
                return default;
            }

            return new AsyncApiJsonDocumentReader(this.settings).ReadFragment<T>(jsonNode, version,
                out diagnostic);
        }

        /// <summary>
        /// Helper method to turn streams into YamlDocument.
        /// </summary>
        /// <param name="input">Stream containing YAML formatted text.</param>
        /// <returns>Instance of a YamlDocument.</returns>
        static JsonNode LoadYamlDocument(TextReader input)
        {
            var yamlStream = new YamlStream();
            yamlStream.Load(input);
            return yamlStream.Documents.First().ToJsonNode();
        }
    }
}
