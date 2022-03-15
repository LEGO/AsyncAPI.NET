namespace LEGO.AsyncAPI.Writers.Example
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Readers;

    public class Program
    {
        static async Task Main()
        {
            var stream = typeof(Program).Assembly.GetManifestResourceStream(typeof(Program).Assembly
                .GetManifestResourceNames().First(x => x.EndsWith("CompleteKafkaSpec.json")));

            var completeKafkaSpecJson = await new StreamReader(stream).ReadToEndAsync();

            var asyncApiDocument = new AsyncApiStringReader().Read(completeKafkaSpecJson, out var readDiagnostic);

            var serializedAsyncApiDocument = new AsyncApiStringWriter().Write(asyncApiDocument, out var writeDiagnostic);

            if (writeDiagnostic.HasError)
            {
                Console.WriteLine($"Error during writing AsyncApiDocument to JSON: {writeDiagnostic.Error}");
            }
            else
            {
                Console.Write(serializedAsyncApiDocument);
            }
        }
    }
}