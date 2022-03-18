namespace LEGO.AsyncAPI.Tests.Models.ChannelBindings
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

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
            var extensions = new Dictionary<string, IAny> { { "x-ext-string", new AsyncAPIString { Value = "foo" } } };
            var output = GetOutputFor(new Dictionary<string, IChannelBinding>() { { "kafka", new KafkaChannelBinding() { Extensions = extensions } } });
            Assert.Equal(
                @"{
  ""$id"": ""1"",
  ""kafka"": {
    ""$id"": ""2"",
    ""x-ext-string"": ""foo""
  }
}", output);
        }

        [Fact]
        public void ShouldProduceObjectWithoutExtensionData()
        {
            var output = GetOutputFor(new Dictionary<string, IChannelBinding>() { { "kafka", new KafkaChannelBinding() { Extensions = null } } });
            Assert.Equal(
                @"{
  ""$id"": ""1"",
  ""kafka"": {
    ""$id"": ""2""
  }
}", output);
        }

        private static string GetOutputFor(IDictionary<string, IChannelBinding>? input)
        {
            return new JsonStringWriter<IDictionary<string, IChannelBinding>?>().Write(input);
        }
    }
}
