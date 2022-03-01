// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Serializers
{
    using YamlDotNet.Serialization;

    internal class YamlToJsonSerializer
    {
        private readonly IDeserializer _deserializer;
        private ISerializer _serializer;

        public YamlToJsonSerializer()
        {
            _deserializer = new Deserializer();

            _serializer = new SerializerBuilder().JsonCompatible().Build();
        }

        public string Serialize(string input)
        {
            var yamlObject = _deserializer.Deserialize<object>(input);

            return _serializer.Serialize(yamlObject);
        }
    }
}