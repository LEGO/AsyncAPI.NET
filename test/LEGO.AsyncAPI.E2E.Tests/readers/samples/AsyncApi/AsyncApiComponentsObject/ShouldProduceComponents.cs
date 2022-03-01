namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiComponentsObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

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