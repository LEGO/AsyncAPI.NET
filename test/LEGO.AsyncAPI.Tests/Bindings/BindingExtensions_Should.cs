// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.WebSockets
{
    using System.Linq;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings.MQTT;
    using LEGO.AsyncAPI.Bindings.Pulsar;
    using LEGO.AsyncAPI.Bindings.WebSockets;
    using LEGO.AsyncAPI.Models;
    using NUnit.Framework;

    public class BindingExtensions_Should
    {
        [Test]
        public void TryGetValue_WithChannelBinding_ReturnsBinding()
        {
            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new WebSocketsChannelBinding
            {
                Method = "POST",
                Query = new AsyncApiSchema
                {
                    Description = "this mah query",
                },
                Headers = new AsyncApiSchema
                {
                    Description = "this mah binding",
                },
            });

            var result = channel.Bindings.TryGetValue<WebSocketsChannelBinding>(out var channelBinding);
            result.Should().BeTrue();
            channelBinding.Should().NotBeNull();
            channelBinding.Should().BeEquivalentTo(channel.Bindings.First().Value);
        }

        [Test]
        public void TryGetValue_WithServerBinding_ReturnsBinding()
        {
            var server = new AsyncApiServer();
            server.Bindings.Add(new PulsarServerBinding
            {
                Tenant = "test tenant",
            });

            var result = server.Bindings.TryGetValue<PulsarServerBinding>(out var serverBinding);
            result.Should().BeTrue();
            serverBinding.Should().NotBeNull();
            serverBinding.Should().BeEquivalentTo(server.Bindings.First().Value);
        }

        [Test]
        public void TryGetValue_WithOperationBinding_ReturnsBinding()
        {
            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new MQTTOperationBinding
            {
                QoS = 23,
                MessageExpiryInterval = 1,
                Retain = true,
            });

            var result = operation.Bindings.TryGetValue<MQTTOperationBinding>(out var operationBinding);
            result.Should().BeTrue();
            operationBinding.Should().NotBeNull();
            operationBinding.Should().BeEquivalentTo(operation.Bindings.First().Value);
        }

        [Test]
        public void TryGetValue_WithMessageBinding_ReturnsBinding()
        {
            var message = new AsyncApiMessage();
            message.Bindings.Add(new MQTTMessageBinding
            {
                PayloadFormatIndicator = 2,
                CorrelationData = new AsyncApiSchema
                {
                    Description = "Test",
                },
            });

            var result = message.Bindings.TryGetValue<MQTTMessageBinding>(out var messageBinding);
            result.Should().BeTrue();
            messageBinding.Should().NotBeNull();
            messageBinding.Should().BeEquivalentTo(message.Bindings.First().Value);
        }
    }
}
