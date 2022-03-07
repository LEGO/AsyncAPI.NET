namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiParameterObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Xunit;

    public class ShouldConsumeParameter : ShouldConsumeProduceBase<Parameter>
    {
        public ShouldConsumeParameter() : base(typeof(ShouldConsumeParameter))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.IsType<Parameter>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("bar", output.Description);
            Assert.IsType<Schema>(output.Schema);
            Assert.Equal("$message.payload#/user/id", output.Location);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}