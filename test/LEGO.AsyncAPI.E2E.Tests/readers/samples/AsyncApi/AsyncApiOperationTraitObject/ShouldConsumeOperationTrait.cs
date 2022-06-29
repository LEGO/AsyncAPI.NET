namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOperationTraitObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeOperationTrait : ShouldConsumeProduceBase<AsyncApiOperationTrait>
    {
        public ShouldConsumeOperationTrait()
            : base(typeof(ShouldConsumeOperationTrait))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.IsType<AsyncApiOperationTrait>(output);
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
        }
    }
}