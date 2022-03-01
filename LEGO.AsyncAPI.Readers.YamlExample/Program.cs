namespace LEGO.AsyncAPI.Readers.YamlExample
{
    using System;
    using System.Text;
    using Readers;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/asyncapi/spec/")
            };

            var stream = await httpClient.GetStreamAsync("master/examples/streetlights-kafka.yml");

            var streetlightKafkaSpec = await new StreamReader(stream, Encoding.UTF8).ReadToEndAsync();

            RemoveNonAsciiSymbols(streetlightKafkaSpec);

            var openApiDocument = new AsyncApiStringReader().Read(RemoveNonAsciiSymbols(streetlightKafkaSpec), out var diagnostic);

            if (diagnostic.Error != null)
            {
                Console.WriteLine("Error during spec parsing: ", diagnostic.Error);
            }
            else
            {
                Console.WriteLine("Kafka Starlight spec successfully parsed into AsyncApiDocument object");
                Console.WriteLine($"Api version: {openApiDocument.Asyncapi}");
                Console.WriteLine($"Number of channels: {openApiDocument.Channels.Count}");
            }
        }

        private static string RemoveNonAsciiSymbols(string input) =>
            Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                    ),
                    Encoding.UTF8.GetBytes(input)
                )
            );
    }
}