namespace LEGO.AsyncAPI.Readers.JsonExample
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class Program
    {
        static async Task Main()
        {
            var stream = typeof(Program).Assembly.GetManifestResourceStream(typeof(Program).Assembly
                .GetManifestResourceNames().First(x => x.EndsWith("CompleteKafkaSpec.json")));

            var asyncApiDocument = new AsyncApiStringReader().Read(await new StreamReader(stream).ReadToEndAsync(), out var diagnostic);

            if (diagnostic.Errors.Any())
            {
                Console.WriteLine($"Error during spec parsing: {diagnostic.Errors.First()}");
            }
            else
            {
                Console.WriteLine("Kafka Starlight JSON spec successfully parsed into AsyncApiDocument object");
                Console.WriteLine($"Api version: {asyncApiDocument.Asyncapi}");
                Console.WriteLine($"Number of channels: {asyncApiDocument.Channels.Count}");
            }
        }
    }
}