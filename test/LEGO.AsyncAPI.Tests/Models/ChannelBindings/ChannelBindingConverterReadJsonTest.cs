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

public class ChannelBindingConverterReadJsonTest
{
    [Fact]
    public void ShouldMapNull()
    {
        Assert.Null(GetOutputFor("null"));
    }

    [Fact]
    public void ShouldConsumeObject()
    {
        var output = GetOutputFor("{\"kafka\":{}}");
        Assert.IsAssignableFrom<IDictionary<string, IChannelBinding>>(output);
    }

    private static IDictionary<string, IChannelBinding>? GetOutputFor(string input)
    {
        var converter = new ChannelBindingConverter();
        var jsonTextReader = new JsonTextReader(new StringReader(input));
        jsonTextReader.Read();
        var output = converter.ReadJson(jsonTextReader, typeof(IDictionary<string, IChannelBinding>), null, JsonSerializer.CreateDefault()) as IDictionary<string, IChannelBinding>;
        return output;
    }
}