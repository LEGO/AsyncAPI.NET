namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiTagObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Xunit;

    public class ShouldConsumeTag: ShouldConsumeProduceBase<Tag>
    {
        public ShouldConsumeTag(): base(typeof(ShouldConsumeTag))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(asyncApiAsyncApiReader.Read(GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));
        
            Assert.Equal("foo", output.Name);
            Assert.Equal("bar", output.Description);
            Assert.IsType<ExternalDocumentation>(output.ExternalDocs);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}