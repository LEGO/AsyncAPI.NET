namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiParameterObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceParameter : ShouldConsumeProduceBase<Parameter>
    {
        public ShouldProduceParameter()
            : base(typeof(ShouldProduceParameter))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new Parameter()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new Parameter
            {
                Description = "bar",
                Schema = new Schema(),
                Location = "$message.payload#/user/id",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}