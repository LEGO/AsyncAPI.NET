namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiServerObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceServer : ShouldConsumeProduceBase<AsyncApiServer>
    {
        public ShouldProduceServer()
            : base(typeof(ShouldProduceServer))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new AsyncApiServer("https://lego.com", "http")));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(MockData.Server()));
        }
    }
}