using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiServerObject
{
    public class ShouldProduceServer: ShouldConsumeProduceBase<Server>
    {
        public ShouldProduceServer(): base(typeof(ShouldProduceServer))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Server("https://lego.com", "http")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new Server("https://lego.com", "http")
            {
                ProtocolVersion = "0.0.1",
                Description = "foo",
                Variables = ImmutableDictionary<string, ServerVariable>.Empty,
                Security = ImmutableDictionary<string, string[]>.Empty,
                Bindings = new Dictionary<string, IServerBinding>
                {
                    {"kafka", new KafkaServerBinding {Extensions = new Dictionary<string, IAny>{{"x-ext-string", new String {Value = "foo"}}}}}
                },
                Extensions = MockData.Extensions()
            }));
        }
    }
}