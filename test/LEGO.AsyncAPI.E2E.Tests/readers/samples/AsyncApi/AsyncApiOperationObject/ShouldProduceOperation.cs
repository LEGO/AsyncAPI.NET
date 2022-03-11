namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOperationObject
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceOperation : ShouldConsumeProduceBase<Operation>
    {
        public ShouldProduceOperation()
            : base(typeof(ShouldProduceOperation))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new Operation()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new Operation
            {
                OperationId = "foo",
                Summary = "bar",
                Description = "baz",
                Tags = ImmutableList<Tag>.Empty,
                ExternalDocs = new ExternalDocumentation(),
                Bindings = MockData.OperationBindings(),
                Traits = ImmutableList<OperationTrait>.Empty,
                Message = new Message(),
                Extensions = MockData.Extensions(),
            }));
        }
    }
}