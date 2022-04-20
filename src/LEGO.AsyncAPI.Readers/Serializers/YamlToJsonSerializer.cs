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

        /// <summary>
        /// Initializes a new instance of YamlToJsonSerializer class.
        /// </summary>
        public YamlToJsonSerializer()
        {
            this.deserializer = new DeserializerBuilder().WithNodeTypeResolver(new PrimitiveTypeNodeResolver()).Build();
            this.serializer = new SerializerBuilder().JsonCompatible().Build();
        }

        /// <summary>
        /// Serialize YAML string into JSON. Also removes non-ascii symbols before serialization to JSON.
        /// </summary>
        /// <param name="input">YAML input as string.</param>
        /// <returns>JSON string with non-ascii symbols removed.</returns>
        public string Serialize(string input)
        {
            var yamlObject = this.deserializer.Deserialize<object>(input.RemoveNonAsciiSymbols());
            return this.serializer.Serialize(yamlObject);
        }
    }
}