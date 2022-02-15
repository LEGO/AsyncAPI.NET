using System.Collections.Generic;
using System.IO;
using System.Text;
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
        var output = GetOutputFor("{\"kafka\":{\"x-ext-string\":\"foo\"}}");
        Assert.IsAssignableFrom<IDictionary<string, KafkaChannelBinding>>(output);
        Assert.IsAssignableFrom<IDictionary<string, IAny>>(output["kafka"].Extensions);
    }

    private static IDictionary<string, KafkaChannelBinding>? GetOutputFor(string input)
    {
        return new JsonAsyncApiReader<IDictionary<string, KafkaChannelBinding>>().Read(new MemoryStream(Encoding.UTF8.GetBytes(input)));
    }
}