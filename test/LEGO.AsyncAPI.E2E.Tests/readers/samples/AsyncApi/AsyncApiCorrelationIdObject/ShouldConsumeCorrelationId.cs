namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiCorrelationIdObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Xunit;

    public class ShouldConsumeCorrelationId: ShouldConsumeProduceBase<CorrelationId>
    {
        public ShouldConsumeCorrelationId(): base(typeof(ShouldConsumeCorrelationId))
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
        
            Assert.Equal("foo", output.Description);
            Assert.Equal("bar", output.Location);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}