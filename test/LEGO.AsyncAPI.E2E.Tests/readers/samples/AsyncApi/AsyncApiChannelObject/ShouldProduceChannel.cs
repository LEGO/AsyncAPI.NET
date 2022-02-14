using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiChannelObject
{
    public class ShouldProduceChannel : ShouldConsumeProduceBase<Channel>
    {
        public ShouldProduceChannel() : base(typeof(ShouldProduceChannel))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Produce(new Channel()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Produce(new Channel()
            {
                Description = "foo",
                Servers = ImmutableList<string>.Empty,
                Subscribe = new Operation(),
                Publish = new Operation(),
                Parameters = ImmutableDictionary<string, Parameter>.Empty,
                Bindings = new Dictionary<string, IChannelBinding>()
                {
                    {"kafka", new KafkaBinding()}
                },
                Extensions = MockData.Extensions()
            }));
        }
    }

    public class KafkaBinding : IChannelBinding
    {
        public IDictionary<string, JToken> Extensions { get; set; }
    }
}