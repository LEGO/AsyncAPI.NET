namespace LEGO.AsyncAPI.Readers;

using System.Text;
using Models;
using Serializers;

public class AsyncApiStreamReader : Interface.IAsyncApiReader<Stream>
{
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