// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Serializers
{
    using YamlDotNet.Serialization;

    /// <summary>
    /// Class responsible for serialization from YAML string to JSON string using YamlDotNet lib.
    /// </summary>
    internal class YamlToJsonSerializer
    {
        private readonly IDeserializer deserializer;
        private readonly ISerializer serializer;

        public YamlToJsonSerializer()
        {
            this.deserializer = new Deserializer();
            this.serializer = new SerializerBuilder().JsonCompatible().Build();
        }

        /// <summary>
        /// Serialize YAML string into JSON.
        /// </summary>
        /// <param name="input">YAML input as string.</param>
        /// <returns>JSON string.</returns>
        public string Serialize(string input)
        {
            var yamlObject = this.deserializer.Deserialize<object>(input.RemoveNonAsciiSymbols());
            return this.serializer.Serialize(yamlObject);
        }
    }
}