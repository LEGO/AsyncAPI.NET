namespace LEGO.AsyncAPI.Readers.JsonExample
{
    using System;

    public class Program
    {
        static async Task Main()
        {
            var stream = typeof(Program).Assembly.GetManifestResourceStream(typeof(Program).Assembly
                .GetManifestResourceNames().First(x => x.EndsWith("CompleteKafkaSpec.json")));

            var openApiDocument = new AsyncApiStringReader().Read(await new StreamReader(stream).ReadToEndAsync(), out var diagnostic);

            if (diagnostic.HasError)
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
    }
}