using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Tests;
using Newtonsoft.Json.Linq;
using Xunit;
using Double = LEGO.AsyncAPI.Any.Double;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiMessageObject
{
    public class ShouldProduceMessage: ShouldConsumeProduceBase<Message>
    {
        public ShouldProduceMessage(): base(typeof(ShouldProduceMessage))
        {
        }

        [Fact]
        public async void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Message()));
        }
        
        [Fact]
        public async void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetString("Complete.json"), _asyncApiWriter.Write(new Message()
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
                Traits = new List<MessageTrait>(),
                ExternalDocs = new ExternalDocumentation(),
                Tags = ImmutableArray<Tag>.Empty,
                Bindings = ImmutableDictionary<string, IMessageBinding>.Empty,
                Examples = ImmutableList<MessageExample>.Empty
            }));
        }
    }
}