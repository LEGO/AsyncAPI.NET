using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
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
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Produce(new Message()));
        }
        
        [Fact]
        public async void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetString("Complete.json"), _asyncApiWriter.Produce(new Message()
            {
                Name = "UserSignup",
                Title = "User signup",
                Summary = "Action to sign a user up.",
                Description = "A longer description",
                ContentType = "application/json",
                Headers = new Schema(),
                Payload = new Double(){ Value = 13.13 },
                SchemaFormat = "application/vnd.aai.asyncapi;version=2.3.0",
                CorrelationId = new CorrelationId(),
                Traits = new List<MessageTrait>(),
                ExternalDocs = new ExternalDocumentation(),
                Tags = ImmutableArray<Tag>.Empty,
                Bindings = ImmutableDictionary<string, IMessageBinding>.Empty,
                Examples = ImmutableList<MessageExample>.Empty
            }));
        }
    }
}