namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiContactObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceContact: ShouldConsumeProduceBase<Contact>
    {
        public ShouldProduceContact(): base(typeof(ShouldProduceContact))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new Contact()));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(MockData.Contact()));
        }
    }
}