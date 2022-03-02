namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiExternalDocsObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceExternalDocs: ShouldConsumeProduceBase<ExternalDocumentation>
    {
        public ShouldProduceExternalDocs(): base(typeof(ShouldProduceExternalDocs))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new ExternalDocumentation()));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.ExternalDocs()));
        }
    }
}