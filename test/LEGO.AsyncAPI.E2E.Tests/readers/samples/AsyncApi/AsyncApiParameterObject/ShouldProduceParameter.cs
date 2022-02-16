using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiParameterObject
{
    public class ShouldProduceParameter : ShouldConsumeProduceBase<Parameter>
    {
        public ShouldProduceParameter() : base(typeof(ShouldProduceParameter))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Parameter()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new Parameter
            {
                Description = "bar",
                Schema = new Schema(),
                Location = "$message.payload#/user/id",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}