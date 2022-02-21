using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LEGO.AsyncAPI.Converters;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.NewtonUtils;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace LEGO.AsyncAPI.Tests;

public class PayloadConverterWriteJsonTest
{
    [Fact]
    public void ShouldProduceNull()
    {
        Assert.Equal("null", GetOutputFor(new Null()));
    }

    [Fact]
    public void ShouldConsumeObject()
    {
        Object obj = new Object();
        obj.Add("foo", new String(){Value = "bar"});
        obj.Add("baz", new Long(){Value = 13});
        obj.Add("bazz", new Double(){Value = 13.13});
        var grault = new Object();
        grault.Add("garply", new String() { Value = "waldo"});
        obj.Add("grault", grault);
        obj.Add("qux", new Array());
        obj.Add("quux", new Boolean(){Value = true});
        obj.Add("quuz", new Null());
        var output = GetOutputFor(obj);
        Assert.Equal("{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\"},\"qux\":[],\"quux\":true,\"quuz\":null}", output);
    }
    
    [Fact]
    public void ShouldProduceString()
    {
        var output = GetOutputFor(new String(){Value = "foo"});
        
        Assert.Equal("\"foo\"", output);
    }
    
    [Fact]
    public void ShouldProduceDouble()
    {
        var output = GetOutputFor(new Double(){Value = 13.13});
        
        Assert.Equal("13.13", output);
    }
    
    [Fact]
    public void ShouldProduceLong()
    {
        var output = GetOutputFor(new Long(){Value = 134341421});
        
        Assert.Equal("134341421", output);
    }
    
    [Fact]
    public void ShouldProduceArray()
    {
        var output = GetOutputFor(new Array { 
            new String {Value = "foo"},
            new String {Value = "bar"},
            new Double {Value = 13.13},
            new Object ()
        });
        
        Assert.Equal("[\"foo\",\"bar\",13.13,{}]", output);
    }
    
    [Fact]
    public void ShouldProduceBoolean()
    {
        var output = GetOutputFor(new Boolean(){Value = true});
        
        Assert.Equal("true", output);
    }

    private static string GetOutputFor(IAny input)
    {
        var converter = new IAnyConverter();
        var stringWriter = new StringWriter();
        JsonWriter jsonTextWriter = new JsonTextWriter(stringWriter);
        converter.WriteJson(jsonTextWriter, input, JsonSerializerUtils.GetSerializer());
        return stringWriter.ToString();
    }
}