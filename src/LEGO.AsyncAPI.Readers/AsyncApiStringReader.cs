// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Text;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Serializers;

    /// <summary>
    /// Converts contents on JSON/YAML string into AsyncApiDocument instance.
    /// </summary>
    internal class AsyncApiStringReader : IAsyncApiReader<string>
    {
        /// <summary>
        /// Reads the string input and parses it into an AsyncApiDocument.
        /// </summary>
        /// <param name="input">String containing AsyncApi definition to parse. Supports JSON and YAML formats.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(string input, out AsyncApiDiagnostic diagnostic)
        {
            var yamlToJsonSerializer = new YamlToJsonSerializer();

            var jsonObject = yamlToJsonSerializer.Serialize(input);

            var apiAsyncJsonReader = new AsyncApiJsonReader();

            return apiAsyncJsonReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(jsonObject)), out diagnostic);
        }
    }
}