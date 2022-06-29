namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOperationObject
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceOperation : ShouldConsumeProduceBase<AsyncApiOperation>
    {
        public ShouldProduceOperation()
            : base(typeof(ShouldProduceOperation))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new AsyncApiOperation()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new AsyncApiOperation
            {
                OperationId = "foo",
                Summary = "bar",
                Description = "baz",
                Tags = ImmutableList<AsyncApiTag>.Empty,
                ExternalDocs = new AsyncApiExternalDocumentation(),
                Bindings = MockData.OperationBindings(),
                Traits = ImmutableList<AsyncApiOperationTrait>.Empty,
                Message = new AsyncApiMessage(),
                Extensions = MockData.Extensions(),
            }));
        }
    }
}