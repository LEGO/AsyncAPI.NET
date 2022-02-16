namespace LEGO.AsyncAPI.Readers.Serializers;

using YamlDotNet.Serialization;

public class YamlToJsonSerializer
{
    public string Serialize(string input)
    {
        var deserializer = new Deserializer();
        var yamlObject = deserializer.Deserialize<object>(input);

        var serializer = new SerializerBuilder().JsonCompatible().Build();
            
        return serializer.Serialize(yamlObject);
    }
}