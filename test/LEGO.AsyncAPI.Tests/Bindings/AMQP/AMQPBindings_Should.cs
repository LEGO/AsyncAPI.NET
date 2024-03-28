// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.AMQP
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.AMQP;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    public class AMQPBindings_Should
    {
        [Test]
        public void AMQPChannelBinding_WithRoutingKey_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  amqp:
    is: routingKey
    exchange:
      name: myExchange
      type: topic
      durable: true
      autoDelete: false
      vhost: /";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new AMQPChannelBinding
            {
                Is = ChannelType.RoutingKey,
                Exchange = new Exchange
                {
                    Name = "myExchange",
                    Type = ExchangeType.Topic,
                    Durable = true,
                    AutoDelete = false,
                    Vhost = "/",
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.AMQP;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }

        [Test]
        public void AMQPChannelBinding_WithQueue_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  amqp:
    is: queue
    queue:
      name: my-queue-name
      durable: true
      exclusive: true
      autoDelete: false
      vhost: /";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new AMQPChannelBinding
            {
                Is = ChannelType.Queue,
                Queue = new Queue
                {
                    Name = "my-queue-name",
                    Durable = true,
                    Exclusive = true,
                    AutoDelete = false,
                    Vhost = "/",
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.AMQP;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }

        [Test]
        public void AMQPMessageBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  amqp:
    contentEncoding: gzip
    messageType: user.signup";

            var message = new AsyncApiMessage();

            message.Bindings.Add(new AMQPMessageBinding
            {
                ContentEncoding = "gzip",
                MessageType = "user.signup",
            });

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.AMQP;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(message);
        }

        [Test]
        public void AMQPOperationBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  amqp:
    expiration: 100000
    userId: guest
    cc:
      - user.logs
    priority: 10
    deliveryMode: 2
    mandatory: false
    bcc:
      - external.audit
    timestamp: true
    ack: false";

            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new AMQPOperationBinding
            {
                Expiration = 100000,
                UserId = "guest",
                Cc = new List<string> { "user.logs" },
                Priority = 10,
                DeliveryMode = DeliveryMode.Persistent,
                Mandatory = false,
                Bcc = new List<string> { "external.audit" },
                Timestamp = true,
                Ack = false,
            }); ;

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.AMQP;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(operation);
        }
    }
}
