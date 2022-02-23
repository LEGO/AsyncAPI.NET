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
        public async void JsonPropertyMinimalSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));
        
            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }
        
        [Fact]
        public async void JsonPropertyCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));
        
            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }
        
        [Fact]
        public async void JsonPropertyCompleteUsingComponentReferencesSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("CompleteUsingComponentReferences.json"));
            
            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);

            // TODO Fix resolving $ref as link to $id, when $ref is pre-populated through JTokenExtensions.ResolveReferences
            //Assert.IsType<KafkaServerBinding>(output.Servers["production"].Bindings["kafka"]);
        }

        [Fact]
        public async void CompleteJsonWithKafkaSpecExampleData()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("CompleteKafkaSpec.json"));

            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal(4, output.Channels.Count);
            Assert.True(output.Channels.TryGetValue("smartylighting.streetlights.1.0.event.{streetlightId}.lighting.measured", out var channel));
            Assert.Equal("The topic on which measured values may be produced and consumed.", channel.Description);
            Assert.Equal("The ID of the streetlight.", channel.Parameters["streetlightId"].Description);
            Assert.Equal("Inform about environmental lighting conditions of a particular streetlight.", channel.Publish.Summary);
            Assert.Equal("receiveLightMeasurement", channel.Publish.OperationId);
            Assert.True(channel.Publish.Traits.First().Bindings.TryGetValue("kafka", out var binding));
            Assert.Equal("string", ((KafkaOperationBinding)binding)?.ClientId.Type);
            Assert.Equal("my-app-id", ((KafkaOperationBinding)binding)?.ClientId.Enum.First());
            Assert.Equal("lightMeasured", channel.Publish.Message.Name);
            Assert.Equal("Light measured", channel.Publish.Message.Title);
        }
    }
}