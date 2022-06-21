namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi
{
    using System;
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using Xunit;

    public class ShouldConsumeAsyncApiDocument : ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldConsumeAsyncApiDocument()
            : base(typeof(ShouldConsumeAsyncApiDocument))
        {
        }

        [Fact]
        public void JsonPropertyMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }

        [Fact]
        public void JsonPropertyCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }

        [Fact]
        public void JsonPropertyCompleteUsingComponentReferencesSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("CompleteUsingComponentReferences.json"));

            Assert.Equal("2.3.0", output.Asyncapi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);

            // TODO Fix resolving $ref as link to $id, when $ref is pre-populated through JTokenExtensions.ResolveReferences
            // Assert.IsType<KafkaServerBinding>(output.Servers["production"].Bindings["kafka"]);
        }

        [Fact]
        public void CompleteJsonWithKafkaSpecExampleData()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("CompleteKafkaSpec.json"));

            this.AssertKafkaSpecExample(output);
        }

        [Fact]
        public void CompleteJsonWithMissingRefKafkaSpecExampleData()
        {
            Assert.Throws<Exception>(() => this.AsyncApiAsyncApiReader.Read(this.GetStream("MissingRefCompleteKafkaSpec.json")));
        }

        [Fact]
        public void CompleteJsonWithKafkaSpecExampleDataDeserializedTwice()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("CompleteKafkaSpec.json"));

            var serializedDoc = this.AsyncApiWriter.Write(output);

            var output2 = this.AsyncApiAsyncApiReader.Read(GenerateStreamFromString(serializedDoc));

            this.AssertKafkaSpecExample(output2);
        }

        private void AssertKafkaSpecExample(AsyncApiDocument doc)
        {
            Assert.Equal("2.3.0", doc.Asyncapi);
            Assert.Equal(4, doc.Channels.Count);
            Assert.True(doc.Channels.TryGetValue("smartylighting.streetlights.1.0.event.{streetlightId}.lighting.measured", out var channel));
            Assert.NotNull(channel);
            Assert.Equal("The topic on which measured values may be produced and consumed.", channel.Description);
            Assert.Equal("The ID of the streetlight.", channel.Parameters["streetlightId"].Description);
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