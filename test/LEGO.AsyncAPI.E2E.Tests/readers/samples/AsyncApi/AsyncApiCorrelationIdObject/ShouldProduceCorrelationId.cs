using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiCorrelationIdObject
{
    public class ShouldProduceCorrelationId: ShouldConsumeProduceBase<CorrelationId>
    {
        public ShouldProduceCorrelationId(): base(typeof(ShouldProduceCorrelationId))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new CorrelationId("bar")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new CorrelationId("bar")
            {
                Description = "foo",
                Extensions = MockData.Extensions()
            }));
        }
    }
}