namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiParameterObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeParameter : ShouldConsumeProduceBase<Parameter>
    {
        public ShouldConsumeParameter()
            : base(typeof(ShouldConsumeParameter))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.IsType<Parameter>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("bar", output.Description);
            Assert.IsType<Schema>(output.Schema);
            Assert.Equal("$message.payload#/user/id", output.Location);
            Assert.IsAssignableFrom<IDictionary<string, IAsyncApiAny>>(output.Extensions);
        }
    }
}