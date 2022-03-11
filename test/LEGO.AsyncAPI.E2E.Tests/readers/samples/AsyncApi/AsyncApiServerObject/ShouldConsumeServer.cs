namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiServerObject
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeServer : ShouldConsumeProduceBase<Server>
    {
        public ShouldConsumeServer()
            : base(typeof(ShouldConsumeServer))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.Equal(new Uri("https://lego.com"), output.Url);
            Assert.Equal("http", output.Protocol);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal(new Uri("https://lego.com"), output.Url);
            Assert.Equal("http", output.Protocol);
            Assert.Equal("0.0.1", output.ProtocolVersion);
            Assert.Equal("foo", output.Description);
            Assert.IsType<Dictionary<string, ServerVariable>>(output.Variables);
            Assert.IsType<List<Dictionary<string, string[]>>>(output.Security);
            Assert.IsType<Dictionary<string, IServerBinding>>(output.Bindings);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}