using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests
{
    public class JsonPropertyMessage
    {
        private IReader<Message> _asyncApiReader;

        public JsonPropertyMessage()
        {
            _asyncApiReader = new AsyncApiReaderNewtonJson<Message>();
        }

        private const string SampleFolderPath = "readers/samples/AsyncApi/AsyncApiMessageObject/";
        
        [Fact]
        public async void JsonPropertyMinimalJsonSpec()
        {
            var output = _asyncApiReader.Consume(Helper.ReadFileToStream(typeof(JsonPropertyInfo), SampleFolderPath, "Minimal.json"));
        
            Assert.NotNull(output);
        }

        [Fact]
        public async void JsonPropertyCompleteSpec()
        {
            var output = _asyncApiReader.Consume(Helper.ReadFileToStream(typeof(JsonPropertyInfo), SampleFolderPath, "Complete.json"));
        
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