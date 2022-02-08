using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace LEGO.AsyncAPI.Tests;

public class PayloadConverterReadJsonTest
{
    [Fact]
    public void ShouldMapNull()
    {
        Assert.IsType<Null>(GetOutputFor("null"));
    }

    [Fact]
    public void ShouldConsumeObject()
    {
        var output = GetOutputFor("{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\"},\"qux\":[],\"quux\":true,\"quuz\":null}");
        Assert.IsType<Object>(output);
        var obj = output as Object;
        Assert.IsType<String>(obj["foo"]);
        Assert.IsType<Long>(obj["baz"]);
        Assert.IsType<Double>(obj["bazz"]);
        Assert.IsType<Array>(obj["qux"]);
        Assert.IsType<Boolean>(obj["quux"]);
        Assert.IsType<Null>(obj["quuz"]);
        Assert.IsType<Object>(obj["grault"]);
        var grault = obj["grault"] as Object;
        Assert.IsType<String>(grault["garply"]);
    }
    
    [Fact]
    public void ShouldConsumeString()
    {
        var output = GetOutputFor("\"foo\"");
        
        Assert.IsType<String>(output);
        Assert.Equal("foo", (output as String)?.Value);
    }
    
    [Fact]
    public void ShouldConsumeDouble()
    {
        var output = GetOutputFor("13.13");
        
        Assert.IsType<Double>(output);
        Assert.Equal(13.13, (output as Double)?.Value);
    }
    
    [Fact]
    public void ShouldConsumeLong()
    {
        var output = GetOutputFor("134341421");
        
        Assert.IsType<Long>(output);
        Assert.Equal(134341421, (output as Long)?.Value);
    }
    
    [Fact]
    public void ShouldConsumeArray()
    {
        var output = GetOutputFor("[ \"foo\", \"bar\", 13.13, {} ]");
        
        Assert.IsType<Array>(output);
        var array = (output as Array);
        Assert.Equal(4, array!.Count);
        Assert.Equal("foo", (array[0] as String)!.Value);
        Assert.Equal(13.13, (array[2] as Double)!.Value);
        Assert.IsType<Object>(array[3]);
    }
    
    [Fact]
    public void ShouldConsumeBoolean()
    {
        var outputTrue = GetOutputFor("true");
        
        Assert.IsType<Boolean>(outputTrue);
        Assert.True((outputTrue as Boolean)!.Value);
        
        var outputFalse = GetOutputFor("false");

        Assert.IsType<Boolean>(outputFalse);
        Assert.False((outputFalse as Boolean)!.Value);
    }

    private static IAny? GetOutputFor(string input)
    {
        var converter = new PayloadConverter();
        var options = new JsonReaderOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        };
        var jsonTextReader = new JsonTextReader(new StringReader(input));
        jsonTextReader.Read();
        var output = converter.ReadJson(jsonTextReader, typeof(IAny), null, JsonSerializer.CreateDefault()) as IAny;
        return output;
    }
}