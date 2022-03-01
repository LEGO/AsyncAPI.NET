namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiTagObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceTag: ShouldConsumeProduceBase<Tag>
    {
        public ShouldProduceTag(): base(typeof(ShouldProduceTag))
        {
        }

        [Fact]
        public async void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Tag()));
        }
        
        [Fact]
        public async void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.Tag()));
        }
    }
}