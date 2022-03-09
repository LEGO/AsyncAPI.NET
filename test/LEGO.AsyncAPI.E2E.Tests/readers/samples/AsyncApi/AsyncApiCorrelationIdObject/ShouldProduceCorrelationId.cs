namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiCorrelationIdObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceCorrelationId: ShouldConsumeProduceBase<CorrelationId>
    {
        public ShouldProduceCorrelationId(): base(typeof(ShouldProduceCorrelationId))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new CorrelationId("bar")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(new CorrelationId("bar")
            {
                Description = "foo",
                Extensions = MockData.Extensions()
            }));
        }
    }
}