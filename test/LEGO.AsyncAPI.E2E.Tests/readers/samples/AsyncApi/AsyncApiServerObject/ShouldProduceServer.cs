namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiServerObject
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
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new Server("https://lego.com", "http")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(MockData.Server()));
        }
    }
}