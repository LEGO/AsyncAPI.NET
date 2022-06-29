namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiParameterObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceParameter : ShouldConsumeProduceBase<AsyncApiParameter>
    {
        public ShouldProduceParameter()
            : base(typeof(ShouldProduceParameter))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new AsyncApiParameter()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new AsyncApiParameter
            {
                Description = "bar",
                Schema = new Schema(),
                Location = "$message.payload#/user/id",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}