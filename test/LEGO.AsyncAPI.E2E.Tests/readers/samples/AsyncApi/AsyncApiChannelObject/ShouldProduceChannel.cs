namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiChannelObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceChannel : ShouldConsumeProduceBase<Channel>
    {
        public ShouldProduceChannel() : base(typeof(ShouldProduceChannel))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new Channel()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(MockData.Channel()));
        }
    }
}