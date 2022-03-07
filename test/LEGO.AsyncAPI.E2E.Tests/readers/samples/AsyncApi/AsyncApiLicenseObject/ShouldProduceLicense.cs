namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiLicenseObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceLicense: ShouldConsumeProduceBase<License>
    {
        public ShouldProduceLicense(): base(typeof(ShouldProduceLicense))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), asyncApiWriter.Write(new License("Apache 2.0")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), asyncApiWriter.Write(MockData.License()));
        }
    }
}