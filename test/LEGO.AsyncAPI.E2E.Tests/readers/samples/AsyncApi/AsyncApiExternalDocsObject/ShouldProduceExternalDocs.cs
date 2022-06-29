namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiExternalDocsObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceExternalDocs : ShouldConsumeProduceBase<AsyncApiExternalDocumentation>
    {
        public ShouldProduceExternalDocs()
            : base(typeof(ShouldProduceExternalDocs))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new AsyncApiExternalDocumentation()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(MockData.ExternalDocs()));
        }
    }
}