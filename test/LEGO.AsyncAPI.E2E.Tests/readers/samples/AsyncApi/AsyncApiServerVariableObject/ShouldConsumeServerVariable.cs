namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiServerVariableObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using Xunit;

    public class ShouldConsumeServerVariable : ShouldConsumeProduceBase<ServerVariable>
    {
        public ShouldConsumeServerVariable()
            : base(typeof(ShouldConsumeServerVariable))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.IsType<ServerVariable>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal(new List<string> { "foo" }, output.Enum);
            Assert.Equal("bar", output.Default);
            Assert.Equal("baz", output.Description);
            Assert.Equal(new List<string> { "quz" }, output.Examples);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}