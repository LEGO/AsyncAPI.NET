// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Serializers
{
    using YamlDotNet.Serialization;

    /// <summary>
    /// Class responsible for serialization from YAML string to JSON string using YamlDotNet lib.
    /// </summary>
    internal class YamlToJsonSerializer
    {
        private readonly IDeserializer _deserializer;
        private ISerializer _serializer;

        public YamlToJsonSerializer()
        {
            _deserializer = new Deserializer();
            _serializer = new SerializerBuilder().JsonCompatible().Build();
        }

        /// <summary>
        /// Serialize YAML string into JSON.
        /// </summary>
        /// <param name="input">YAML input as string.</param>
        /// <returns>JSON string.</returns>
        public string Serialize(string input)
        {
            var yamlObject = _deserializer.Deserialize<object>(input.RemoveNonAsciiSymbols());
            return _serializer.Serialize(yamlObject);
        }
    }
}