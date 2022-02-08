using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;
using Double = LEGO.AsyncAPI.Any.Double;

namespace LEGO.AsyncAPI.E2E.Tests
{
    public class ShouldProduceMessage
    {
        private IReader<Message> _asyncApiReader;

        public ShouldProduceMessage()
        {
            _asyncApiReader = new AsyncApiReaderNewtonJson<Message>();
        }

        private const string SampleFolderPath = "readers/samples/AsyncApi/AsyncApiMessageObject/";
        
        [Fact]
        public async void JsonPropertyCompleteSpec()
        {
            var input = Helper.ReadFileToStreamAsString(typeof(ShouldProduceAsyncApiDocument), SampleFolderPath, "Complete.json");
            
            Assert.Equal(input, new AsyncApiWriter<Message>().Produce(new Message()
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
                ExternalDocs = new ExternalDocumentation()
            }));
        }
    }
}