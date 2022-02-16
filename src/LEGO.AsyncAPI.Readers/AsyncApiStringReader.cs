namespace LEGO.AsyncAPI.Readers;

using System.Text;
using Models;
using Serializers;

public class AsyncApiStringReader : Interface.IAsyncApiReader<string>
{
    public AsyncApiDocument Read(string input, out AsyncApiDiagnostic diagnostic)
    {
        var yamlToJsonSerializer = new YamlToJsonSerializer();

        var jsonObject = yamlToJsonSerializer.Serialize(input);

        var apiAsyncJsonReader = new AsyncApiJsonReader();

        return apiAsyncJsonReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(jsonObject)), out diagnostic);
    }
}