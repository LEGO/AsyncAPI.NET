namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiMessageTraitObject
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldProduceMessageTrait : ShouldConsumeProduceBase<MessageTrait>
    {
        public ShouldProduceMessageTrait()
            : base(typeof(ShouldProduceMessageTrait))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new MessageTrait()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetString("Complete.json"), this.AsyncApiWriter.Write(new MessageTrait()
            {
                Name = "UserSignup",
                Title = "User signup",
                Summary = "Action to sign a user up.",
                Description = "A longer description",
                ContentType = "application/json",
                Headers = new Schema(),
                SchemaFormat = "application/vnd.aai.asyncapi;version=2.3.0",
                CorrelationId = new CorrelationId("foo"),
                ExternalDocs = new ExternalDocumentation(),
                Tags = ImmutableArray<Tag>.Empty,
                Bindings = new Dictionary<string, IMessageBinding>()
                {
                    {
                        "http", new HttpMessageBinding
                        {
                            Headers = new Schema(),
                            BindingVersion = "foo",
                        }
                    },
                },
                Examples = ImmutableList<MessageExample>.Empty,
            }));
        }
    }
}