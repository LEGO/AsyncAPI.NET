// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.MQTT
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.MQTT;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    public class MQTTBindings_Should
    {
        [Test]
        public void MQTTServerBinding_FilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"url: https://example.com
protocol: mqtt
bindings:
  mqtt:
    clientId: guest
    cleanSession: true
    lastWill:
      topic: /last-wills
      qos: 2
      message: Guest gone offline.
      retain: false
    keepAlive: 60
    sessionExpiryInterval: 600
    maximumPacketSize: 1200";

            var server = new AsyncApiServer();
            server.Url = "https://example.com";
            server.Protocol = "mqtt";
            server.Bindings.Add(new MQTTServerBinding
            {
                ClientId = "guest",
                CleanSession = true,
                LastWill = new LastWill
                {
                    Topic = "/last-wills",
                    QoS = 2,
                    Message = "Guest gone offline.",
                    Retain = false,
                },
                KeepAlive = 60,
                SessionExpiryInterval = 600,
                MaximumPacketSize = 1200,
            });

            // Act
            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.MQTT;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(server);
        }

        [Test]
        public void MQTTOperationBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  mqtt:
    qos: 2
    retain: true
    messageExpiryInterval: 60";

            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new MQTTOperationBinding
            {
                QoS = 2,
                Retain = true,
                MessageExpiryInterval = 60,
            });

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.MQTT;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(operation);
        }

        [Test]
        public void MQTTMessageBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  mqtt:
    correlationData:
      type: string
      format: uuid
    contentType: application/json";

            var message = new AsyncApiMessage();

            message.Bindings.Add(new MQTTMessageBinding
            {
                ContentType = "application/json",
                CorrelationData = new AsyncApiSchema
                {
                    Type = SchemaType.String,
                    Format = "uuid",
                },
            });

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.MQTT;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(message);
        }
    }
}
