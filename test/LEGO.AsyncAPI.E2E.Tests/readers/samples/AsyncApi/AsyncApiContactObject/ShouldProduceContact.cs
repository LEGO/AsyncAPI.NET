namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiContactObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceContact : ShouldConsumeProduceBase<AsyncApiContact>
    {
        public ShouldProduceContact()
            : base(typeof(ShouldProduceContact))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new AsyncApiContact()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(MockData.Contact()));
        }
    }
}