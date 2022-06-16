namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiMessageTraitObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeMessageTrait : ShouldConsumeProduceBase<MessageTrait>
    {
        public ShouldConsumeMessageTrait()
            : base(typeof(ShouldConsumeMessageTrait))
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
            Assert.IsType<CorrelationId>(output.CorrelationId);
            Assert.Equal("application/vnd.aai.asyncapi;version=2.3.0", output.SchemaFormat);
            Assert.Equal("application/json", output.ContentType);
            Assert.Equal("UserSignup", output.Name);
            Assert.Equal("User signup", output.Title);
            Assert.Equal("Action to sign a user up.", output.Summary);
            Assert.Equal("A longer description", output.Description);
            Assert.IsType<List<AsyncApiTag>>(output.Tags);
            Assert.IsType<AsyncApiExternalDocumentation>(output.ExternalDocs);
            var messageBindings = output.Bindings;
            Assert.IsAssignableFrom<IDictionary<string, IMessageBinding>>(messageBindings);
            Assert.IsType<HttpMessageBinding>(messageBindings["http"]);
            Assert.IsType<List<MessageExample>>(output.Examples);
        }
    }
}