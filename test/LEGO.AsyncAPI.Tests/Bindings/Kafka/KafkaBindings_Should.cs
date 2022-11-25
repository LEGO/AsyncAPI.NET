namespace LEGO.AsyncAPI.Tests.Bindings.Kafka
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class KafkaBindings_Should
    {
        [Test]
        public void KafkaServerBinding_FilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"url: https://example.com
protocol: kafka
bindings:
  kafka:
    schemaRegistryUrl: https://example.com/schemaregistry
    schemaRegistryVendor: confluent";

            var server = new AsyncApiServer()
            {
                Url = "https://example.com",
                Protocol = "kafka",
            };

            server.Bindings.Add(new KafkaServerBinding
            {
                SchemaRegistryUrl = "https://example.com/schemaregistry",
                SchemaRegistryVendor = "confluent",
            });

            // Act
            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var binding = new AsyncApiStringReader().ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(server);
        }

        [Test]
        public void KafkaMessageBinding_FilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  kafka:
    key:
      description: this mah other binding
    SchemaIdLocation: test
    schemaIdPayloadEncoding: test
    schemaLookupStrategy: header";

            var message = new AsyncApiMessage();

            message.Bindings.Add(new KafkaMessageBinding
            {
                Key = new AsyncApiSchema
                {
                    Description = "this mah other binding",
                },
                SchemaIdLocation = "test",
                SchemaIdPayloadEncoding = "test",
                SchemaLookupStrategy = "header",
            });

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var binding = new AsyncApiStringReader().ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(message);
        }

        [Test]
        public void KafkaOperationBinding_FilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  kafka:
    groupId:
      description: this mah groupId
    clientId:
      description: this mah clientId";

            var operation = new AsyncApiOperation();

            operation.Bindings.Add(new KafkaOperationBinding
            {
                GroupId = new AsyncApiSchema
                {
                    Description = "this mah groupId",
                },
                ClientId = new AsyncApiSchema
                {
                    Description = "this mah clientId",
                },
            });

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var binding = new AsyncApiStringReader().ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(operation);
        }
    }
}
