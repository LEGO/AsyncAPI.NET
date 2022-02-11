using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
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
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Produce(new Server("https://lego.com", "http")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Produce(new Server("https://lego.com", "http")
            {
                ProtocolVersion = "0.0.1",
                Description = "foo",
                Variables = ImmutableDictionary<string, ServerVariable>.Empty,
                Security = ImmutableDictionary<string, string[]>.Empty,
                Bindings = ImmutableDictionary<string, IServerBinding>.Empty,
                Extensions = MockData.Extensions()
            }));
        }
    }
}