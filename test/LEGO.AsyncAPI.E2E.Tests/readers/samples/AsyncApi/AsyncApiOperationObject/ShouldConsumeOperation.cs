namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOperationObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeOperation : ShouldConsumeProduceBase<AsyncApiOperation>
    {
        public ShouldConsumeOperation()
            : base(typeof(ShouldConsumeOperation))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.IsType<AsyncApiOperation>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.OperationId);
            Assert.Equal("bar", output.Summary);
            Assert.Equal("baz", output.Description);
            Assert.IsAssignableFrom<IList<AsyncApiTag>>(output.Tags);
            Assert.IsType<AsyncApiExternalDocumentation>(output.ExternalDocs);
            Assert.IsAssignableFrom<IDictionary<string, IOperationBinding>>(output.Bindings);
            Assert.IsType<KafkaOperationBinding>(output.Bindings["kafka"]);
            Assert.IsType<HttpOperationBinding>(output.Bindings["http"]);
            Assert.IsType<List<AsyncApiOperationTrait>>(output.Traits);
            Assert.IsType<AsyncApiMessage>(output.Message);
        }
    }
}