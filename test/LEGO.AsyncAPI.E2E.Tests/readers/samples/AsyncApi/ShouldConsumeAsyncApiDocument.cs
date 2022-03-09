namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi
{
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using Models.Bindings.OperationBindings;
    using Xunit;

    public class ShouldConsumeAsyncApiDocument: ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldConsumeAsyncApiDocument(): base(typeof(ShouldConsumeAsyncApiDocument))
        {
        }

        [Fact]
        public void JsonPropertyMinimalSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("Minimal.json"));
        
            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }
        
        [Fact]
        public void JsonPropertyCompleteSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));
        
            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }
        
        [Fact]
        public void JsonPropertyCompleteUsingComponentReferencesSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("CompleteUsingComponentReferences.json"));
            
            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);

            // TODO Fix resolving $ref as link to $id, when $ref is pre-populated through JTokenExtensions.ResolveReferences
            //Assert.IsType<KafkaServerBinding>(output.Servers["production"].Bindings["kafka"]);
        }

        [Fact]
        public void CompleteJsonWithKafkaSpecExampleData()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("CompleteKafkaSpec.json"));

            AssertKafkaSpecExample(output);
        }

        [Fact]
        public void CompleteJsonWithKafkaSpecExampleDataDeserializedTwice()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("CompleteKafkaSpec.json"));

            var serializedDoc = AsyncApiWriter.Write(output);

            var output2 = AsyncApiAsyncApiReader.Read(GenerateStreamFromString(serializedDoc));
            
            AssertKafkaSpecExample(output2);
        }

        private void AssertKafkaSpecExample(AsyncApiDocument doc)
        {
            Assert.Equal("2.3.0", doc.Asyncapi);
            Assert.Equal(4, doc.Channels.Count);
            Assert.True(doc.Channels.TryGetValue("smartylighting.streetlights.1.0.event.{streetlightId}.lighting.measured", out var channel));
            Assert.Equal("The topic on which measured values may be produced and consumed.", channel?.Description);
            Assert.Equal("The ID of the streetlight.", channel?.Parameters["streetlightId"].Description);
            Assert.Equal("Inform about environmental lighting conditions of a particular streetlight.", channel?.Publish.Summary);
            Assert.Equal("receiveLightMeasurement", channel.Publish.OperationId);
            Assert.True(channel.Publish.Traits.First().Bindings.TryGetValue("kafka", out var binding));
            Assert.Equal("string", (binding as KafkaOperationBinding)?.ClientId.Type);
            Assert.Equal("my-app-id", (binding as KafkaOperationBinding)?.ClientId.Enum.First());
            Assert.Equal("lightMeasured", channel.Publish.Message.Name);
            Assert.Equal("Light measured", channel.Publish.Message.Title);
        }
    }
}