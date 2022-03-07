namespace LEGO.AsyncAPI.Readers.YamlExample
{
    using System;
    using System.Text;
    using Readers;

    public class Program
    {
        static async Task Main()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/asyncapi/spec/")
            };

            var stream = await httpClient.GetStreamAsync("master/examples/streetlights-kafka.yml");

            var streetlightKafkaSpec = await new StreamReader(stream, Encoding.UTF8).ReadToEndAsync();

            var openApiDocument = new AsyncApiStringReader().Read(streetlightKafkaSpec, out var diagnostic);

            if (diagnostic.HasError)
            {
                Console.WriteLine($"Error during spec parsing: {diagnostic.Error}");
            }
            else
            {
                Console.WriteLine("Kafka Starlight YAML spec successfully parsed into AsyncApiDocument object");
                Console.WriteLine($"Api version: {openApiDocument.Asyncapi}");
                Console.WriteLine($"Number of channels: {openApiDocument.Channels.Count}");
            }
        }
    }
}