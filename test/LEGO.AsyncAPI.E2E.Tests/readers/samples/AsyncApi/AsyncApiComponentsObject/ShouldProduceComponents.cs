namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiComponentsObject
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
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new Components()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(MockData.Components()));
        }
    }
}