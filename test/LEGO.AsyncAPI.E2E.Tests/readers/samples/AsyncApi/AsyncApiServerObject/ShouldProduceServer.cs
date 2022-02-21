using System.Collections.Generic;
using System.Collections.Immutable;
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
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.Server()));
        }
    }
}