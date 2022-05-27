namespace LEGO.AsyncAPI.Tests.Models.ChannelBindings
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

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
            Assert.IsAssignableFrom<IDictionary<string, IAsyncApiAny>>(output?["kafka"].Extensions);
        }

        private static IDictionary<string, KafkaChannelBinding>? GetOutputFor(string input)
        {
            return new JsonStreamReader<IDictionary<string, KafkaChannelBinding>>().Read(new MemoryStream(Encoding.UTF8.GetBytes(input)));
        }
    }
}
