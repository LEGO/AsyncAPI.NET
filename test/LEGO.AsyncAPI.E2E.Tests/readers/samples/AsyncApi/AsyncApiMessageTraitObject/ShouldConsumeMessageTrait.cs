using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiMessageTraitObject
{
    public class ShouldConsumeMessageTrait: ShouldConsumeProduceBase<MessageTrait>
    {
        public ShouldConsumeMessageTrait(): base(typeof(ShouldConsumeMessageTrait))
        {
        }

        [Fact]
        public async void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(_asyncApiAsyncApiReader.Read(GetStream("Minimal.json")));
        }

        [Fact]
        public async void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Complete.json"));
        
            Assert.IsType<Schema>(output.Headers);
            Assert.IsType<CorrelationId>(output.CorrelationId);
            Assert.Equal("application/vnd.aai.asyncapi;version=2.3.0", output.SchemaFormat);
            Assert.Equal("application/json", output.ContentType);
            Assert.Equal("UserSignup", output.Name);
            Assert.Equal("User signup", output.Title);
            Assert.Equal("Action to sign a user up.", output.Summary);
            Assert.Equal("A longer description", output.Description);
            Assert.IsType<List<Tag>>(output.Tags);
            Assert.IsType<ExternalDocumentation>(output.ExternalDocs);
            var messageBindings = output.Bindings;
            Assert.IsAssignableFrom<IDictionary<string, IMessageBinding>>(messageBindings);
            Assert.IsType<HttpMessageBinding>(messageBindings["http"]);
            Assert.IsType<List<MessageExample>>(output.Examples);
        }
    }
}