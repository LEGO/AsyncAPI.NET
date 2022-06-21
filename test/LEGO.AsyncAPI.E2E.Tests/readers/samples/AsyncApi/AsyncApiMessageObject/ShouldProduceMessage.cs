namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiMessageObject
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceMessage : ShouldConsumeProduceBase<AsyncApiMessage>
    {
        public ShouldProduceMessage()
            : base(typeof(ShouldProduceMessage))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new AsyncApiMessage()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetString("Complete.json"), this.AsyncApiWriter.Write(new AsyncApiMessage
            {
                Name = "UserSignup",
                Title = "User signup",
                Summary = "Action to sign a user up.",
                Description = "A longer description",
                ContentType = "application/json",
                Headers = new Schema(),
                Payload = MockData.Payload(),
                SchemaFormat = "application/vnd.aai.asyncapi;version=2.3.0",
                CorrelationId = new CorrelationId("foo"),
                Traits = new List<AsyncApiMessageTrait>(),
                ExternalDocs = new AsyncApiExternalDocumentation(),
                Tags = ImmutableArray<AsyncApiTag>.Empty,
                Bindings = MockData.MessageBindings(),
                Examples = ImmutableList<AsyncApiMessageExample>.Empty,
            }));
        }
    }
}