using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Json.More;
using Newtonsoft.Json;
using Xunit.Sdk;
using YamlDotNet.Serialization;

namespace LEGO.AsyncAPI.Tests;

public class Helper
{
    public static async Task<JsonDocument> ReadInputStreamToJsonDocument(Type type, string path, string filename)
    {
        var manifestResourceStream = ReadFileToStream(type, path, filename);

        if (manifestResourceStream == null)
        {
            throw new FileNotFoundException("The stream is null because the file was not found");
        }

        try
        {
            return await JsonDocument.ParseAsync(manifestResourceStream!);
        }
        catch (Exception e)
        {
            var deserializer = new DeserializerBuilder().Build(); 
            var yamlObject = deserializer.Deserialize(new StreamReader(ReadFileToStream(type, path, filename)));
            var serializer = new SerializerBuilder().JsonCompatible().Build();
            var serialize = serializer.Serialize(yamlObject);
            var readInputStreamToJsonDocument = await JsonDocument.ParseAsync(new MemoryStream(Encoding.UTF8.GetBytes(serialize ?? "")));
            return readInputStreamToJsonDocument;            
        }
    }

    public static string ReadFileToStreamAsString(Type type, string path, string filename)
    {
        var stream = ReadFileToStream(type, path, filename);
        if (stream == null)
        {
            throw new FileNotFoundException("The stream is null because the file was not found");
        }
        return new StreamReader(stream).ReadToEnd();
    }

    public static Stream? ReadFileToStream(Type type, string path, string filename)
    {
        var combine = type.Namespace + "." + Path.Combine(path, filename).Replace("/", ".");
        Stream? manifestResourceStream = type.Assembly.GetManifestResourceStream(combine);
        return manifestResourceStream;
    }
}