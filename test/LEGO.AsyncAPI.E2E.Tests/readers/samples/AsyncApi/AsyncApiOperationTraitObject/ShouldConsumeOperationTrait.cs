using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOperationTraitObject
{
    public class ShouldConsumeOperationTrait : ShouldConsumeProduceBase<OperationTrait>
    {
        public ShouldConsumeOperationTrait() : base(typeof(ShouldConsumeOperationTrait))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.IsType<OperationTrait>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.OperationId);
            Assert.Equal("bar", output.Summary);
            Assert.Equal("baz", output.Description);
            Assert.IsAssignableFrom<IList<Tag>>(output.Tags);
            Assert.IsType<ExternalDocumentation>(output.ExternalDocs);
            Assert.IsAssignableFrom<IDictionary<string, IOperationBinding>>(output.Bindings);
            Assert.IsType<KafkaOperationBinding>(output.Bindings["kafka"]);
            Assert.IsType<HttpOperationBinding>(output.Bindings["http"]);
        }
    }
}