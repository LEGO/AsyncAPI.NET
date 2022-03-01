namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiChannelObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Models.Interfaces;
    using Xunit;

    public class ShouldConsumeChannel : ShouldConsumeProduceBase<Channel>
    {
        public ShouldConsumeChannel() : base(typeof(ShouldConsumeChannel))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.IsType<Channel>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.Description);
            Assert.IsAssignableFrom<IList<string>>(output.Servers);
            Assert.IsType<Operation>(output.Subscribe);
            Assert.IsType<Operation>(output.Publish);
            Assert.IsAssignableFrom<IDictionary<string, Parameter>>(output.Parameters);
            Assert.IsAssignableFrom<IDictionary<string, IChannelBinding>>(output.Bindings);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}