namespace LEGO.AsyncAPI.Readers.Serializers
{
    using System.Text;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class YamlToJsonSerializer
    {
        public string Serialize(string input)
        {
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize<object>(input);

            var serializer = new JsonSerializer();

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonWriter = new JsonTextWriter(streamWriter);

            serializer.Serialize(jsonWriter, yamlObject);
            jsonWriter.Flush();
            streamWriter.Flush();

            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }
    }
}
