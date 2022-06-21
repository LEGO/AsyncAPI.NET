namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiMessageObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeMessage : ShouldConsumeProduceBase<AsyncApiMessage>
    {
        public ShouldConsumeMessage()
            : base(typeof(ShouldConsumeMessage))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Complete.json"));

            Assert.IsType<Schema>(output.Headers);
            Assert.IsAssignableFrom<IAsyncApiAny>(output.Payload);
            Assert.IsType<AsyncApiCorrelationId>(output.CorrelationId);
            Assert.Equal("application/vnd.aai.asyncapi;version=2.3.0", output.SchemaFormat);
            Assert.Equal("application/json", output.ContentType);
            Assert.Equal("UserSignup", output.Name);
            Assert.Equal("User signup", output.Title);
            Assert.Equal("Action to sign a user up.", output.Summary);
            Assert.Equal("A longer description", output.Description);
            Assert.IsType<List<AsyncApiTag>>(output.Tags);
            Assert.IsType<AsyncApiExternalDocumentation>(output.ExternalDocs);
            Assert.IsAssignableFrom<IDictionary<string, IMessageBinding>>(output.Bindings);
            Assert.IsType<List<AsyncApiMessageExample>>(output.Examples);
            Assert.IsType<List<AsyncApiMessageTrait>>(output.Traits);
        }
    }
}