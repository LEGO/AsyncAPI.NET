namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiCorrelationIdObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceCorrelationId : ShouldConsumeProduceBase<CorrelationId>
    {
        public ShouldProduceCorrelationId()
            : base(typeof(ShouldProduceCorrelationId))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new CorrelationId("bar")));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new CorrelationId("bar")
            {
                Description = "foo",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}