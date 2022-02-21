using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiComponentsObject
{
    public class ShouldProduceComponents : ShouldConsumeProduceBase<Components>
    {
        public ShouldProduceComponents() : base(typeof(ShouldProduceComponents))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Components()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.Components()));
        }
    }
}