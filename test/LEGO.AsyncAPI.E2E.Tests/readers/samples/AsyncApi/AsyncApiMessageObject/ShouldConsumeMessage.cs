namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiMessageObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Models.Interfaces;
    using Xunit;

    public class ShouldConsumeMessage: ShouldConsumeProduceBase<Message>
    {
        public ShouldConsumeMessage(): base(typeof(ShouldConsumeMessage))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(AsyncApiAsyncApiReader.Read(GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("Complete.json"));
        
            Assert.IsType<Schema>(output.Headers);
            Assert.IsAssignableFrom<IAny>(output.Payload);
            Assert.IsType<CorrelationId>(output.CorrelationId);
            Assert.Equal("application/vnd.aai.asyncapi;version=2.3.0", output.SchemaFormat);
            Assert.Equal("application/json", output.ContentType);
            Assert.Equal("UserSignup", output.Name);
            Assert.Equal("User signup", output.Title);
            Assert.Equal("Action to sign a user up.", output.Summary);
            Assert.Equal("A longer description", output.Description);
            Assert.IsType<List<Tag>>(output.Tags);
            Assert.IsType<ExternalDocumentation>(output.ExternalDocs);
            Assert.IsAssignableFrom<IDictionary<string, IMessageBinding>>(output.Bindings);
            Assert.IsType<List<MessageExample>>(output.Examples);
            Assert.IsType<List<MessageTrait>>(output.Traits);
        }
    }
}