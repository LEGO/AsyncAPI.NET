namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOperationObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Bindings.OperationBindings;
    using Models.Interfaces;
    using Xunit;

    public class ShouldConsumeOperation : ShouldConsumeProduceBase<Operation>
    {
        public ShouldConsumeOperation() : base(typeof(ShouldConsumeOperation))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.IsType<Operation>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.OperationId);
            Assert.Equal("bar", output.Summary);
            Assert.Equal("baz", output.Description);
            Assert.IsAssignableFrom<IList<Tag>>(output.Tags);
            Assert.IsType<ExternalDocumentation>(output.ExternalDocs);
            Assert.IsAssignableFrom<IDictionary<string, IOperationBinding>>(output.Bindings);
            Assert.IsType<KafkaOperationBinding>(output.Bindings["kafka"]);
            Assert.IsType<HttpOperationBinding>(output.Bindings["http"]);
            Assert.IsType<List<OperationTrait>>(output.Traits);
            Assert.IsType<Message>(output.Message);
        }
    }
}