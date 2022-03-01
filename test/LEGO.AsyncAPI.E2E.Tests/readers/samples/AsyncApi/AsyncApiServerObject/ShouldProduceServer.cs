namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiServerObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

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