namespace LEGO.AsyncAPI.Readers.JsonExample
{
    using System;
    using System.Text;
    using YamlDotNet.Serialization;

    public class Program
    {
        static async Task Main()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/asyncapi/spec/")
            };

            var stream = await httpClient.GetStreamAsync("master/examples/streetlights-kafka.yml");

            var streetlightKafkaYamlSpec = await new StreamReader(stream, Encoding.UTF8).ReadToEndAsync();

            var streetlightKafkaJsonSpec = ConvertYamlToJson(streetlightKafkaYamlSpec);

            var openApiDocument = new AsyncApiStringReader().Read(streetlightKafkaJsonSpec, out var diagnostic);

            if (diagnostic.Error != null)
            {
                Console.WriteLine($"Error during spec parsing: {diagnostic.Error}");
            }
            else
            {
                Console.WriteLine("Kafka Starlight JSON spec successfully parsed into AsyncApiDocument object");
                Console.WriteLine($"Api version: {openApiDocument.Asyncapi}");
                Console.WriteLine($"Number of channels: {openApiDocument.Channels.Count}");
            }
        }

        private static string ConvertYamlToJson(string input)
        {
            var deserializer = new Deserializer();
            var serializer = new SerializerBuilder().JsonCompatible().Build();
            var yamlObject = deserializer.Deserialize<object>(RemoveNonAscii(input));
            return serializer.Serialize(yamlObject);
        }

        private static string RemoveNonAscii(string input) =>
            Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()),
                    Encoding.UTF8.GetBytes(input)));
    }
}