// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.Kafka
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class KafkaBindings_Should
    {
        [Test]
        public void KafkaChannelBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  kafka:
    topic: myTopic
    partitions: 5
    replicas: 4
    topicConfiguration:
      cleanup.policy:
        - delete
        - compact
      retention.ms: 1
      retention.bytes: 2
      delete.retention.ms: 3
      max.message.bytes: 4";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new KafkaChannelBinding
            {
                Topic = "myTopic",
                Partitions = 5,
                Replicas = 4,
                TopicConfiguration = new TopicConfigurationObject()
                {
                    CleanupPolicy = new List<string> { "delete", "compact" },
                    RetentionMiliseconds = 1,
                    RetentionBytes = 2,
                    DeleteRetentionMiliseconds = 3,
                    MaxMessageBytes = 4,
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(BindingsCollection.Kafka);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }

        [Test]
        public void KafkaServerBinding_WithFilledObject_SerializesAndDeserializes()
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
            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(BindingsCollection.Kafka);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(server);
        }

        [Test]
        public void KafkaMessageBinding_WithFilledObject_SerializesAndDeserializes()
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
            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(BindingsCollection.Kafka);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(message);
        }

        [Test]
        public void KafkaOperationBinding_WithFilledObject_SerializesAndDeserializes()
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

            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(BindingsCollection.Kafka);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(operation);
        }
    }
}
