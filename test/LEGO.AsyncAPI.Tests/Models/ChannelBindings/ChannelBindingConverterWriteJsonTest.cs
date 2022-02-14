using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.ChannelBindings;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace LEGO.AsyncAPI.Tests.Models.ChannelBindings;

public class ChannelBindingConverterWriteJsonTest
{
    [Fact]
    public void ShouldMapNull()
    {
        Assert.Equal("null", GetOutputFor(null));
    }

    [Fact]
    public void ShouldProduceObject()
    {
        var output = GetOutputFor(new Dictionary<string, IChannelBinding>(){{"kafka", new KafkaChannelBinding()}});
        Assert.Equal("{\"kafka\":{}}", output);
    }

    private static string GetOutputFor(IDictionary<string, IChannelBinding> input)
    {
        var converter = new ChannelBindingConverter();
        var stringWriter = new StringWriter();
        JsonWriter jsonTextWriter = new JsonTextWriter(stringWriter);
        converter.WriteJson(jsonTextWriter, input, JsonSerializer.CreateDefault());
        return stringWriter.ToString();
    }
}