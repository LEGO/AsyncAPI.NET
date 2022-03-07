namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOperationObject
{
    using System.Collections.Immutable;
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceOperation : ShouldConsumeProduceBase<Operation>
    {
        public ShouldProduceOperation() : base(typeof(ShouldProduceOperation))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), asyncApiWriter.Write(new Operation()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), asyncApiWriter.Write(new Operation
            {
                OperationId = "foo",
                Summary = "bar",
                Description = "baz",
                Tags = ImmutableList<Tag>.Empty,
                ExternalDocs = new ExternalDocumentation(),
                Bindings = MockData.OperationBindings(),
                Traits = ImmutableList<OperationTrait>.Empty,
                Message = new Message(),
                Extensions = MockData.Extensions()
            }));
        }
    }
}