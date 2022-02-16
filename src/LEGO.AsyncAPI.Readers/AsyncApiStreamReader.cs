namespace LEGO.AsyncAPI.Readers;

using System.Text;
using Models;
using Serializers;

/// <summary>
/// Converts contents on JSON/YAML stream into AsyncApiDocument instance 
/// </summary>
public class AsyncApiStreamReader : Interface.IAsyncApiReader<Stream>
{
    /// <summary>
    /// Reads the stream input and parses it into an AsyncApiDocument.
    /// </summary>
    /// <param name="input">Stream containing AsyncApi definition to parse. Supports JSON and YAML formats.</param>
    /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
    /// <returns>Instance of newly created AsyncApiDocument.</returns>
    public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
    {
        var yamlToJsonSerializer = new YamlToJsonSerializer();

        string jsonObject;

        try
        {
            jsonObject = yamlToJsonSerializer.Serialize(new StreamReader(input, Encoding.UTF8).ReadToEnd());
        }
        catch (Exception e)
        {
            diagnostic = AsyncApiDiagnostic.OnError(e);
            return new AsyncApiDocument();
        }

        var apiAsyncJsonReader = new AsyncApiJsonReader();

        return apiAsyncJsonReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(jsonObject)), out diagnostic);
    }
}