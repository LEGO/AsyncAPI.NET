namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiParameterObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceParameter : ShouldConsumeProduceBase<Parameter>
    {
        public ShouldProduceParameter() : base(typeof(ShouldProduceParameter))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new Parameter()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(new Parameter
            {
                Description = "bar",
                Schema = new Schema(),
                Location = "$message.payload#/user/id",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}