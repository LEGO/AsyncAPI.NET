// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.IO;
    using System.Text;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Serializers;

    /// <summary>
    /// Converts contents on JSON/YAML stream into AsyncApiDocument instance.
    /// </summary>
    public class AsyncApiStreamReader : IAsyncApiReader<Stream>
    {
        private readonly YamlToJsonSerializer serializer;
        private readonly IAsyncApiReader<Stream> jsonAsyncApiReader;

        public AsyncApiStreamReader()
        {
            this.serializer = new YamlToJsonSerializer();
            this.jsonAsyncApiReader = new AsyncApiJsonReader();
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApiDocument.
        /// </summary>
        /// <param name="input">Stream containing AsyncApi definition to parse. Supports JSON and YAML formats.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
        {
            AsyncApiDocument output = new();

            try
            {
                var jsonObject = this.serializer.Serialize(new StreamReader(input, Encoding.UTF8).ReadToEnd());

                output = this.jsonAsyncApiReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(jsonObject)), out diagnostic);
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