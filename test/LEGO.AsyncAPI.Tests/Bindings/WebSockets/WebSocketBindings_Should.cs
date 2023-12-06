﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.WebSockets
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.WebSockets;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class WebSocketBindings_Should
    {
        [Test]
        public void WebSocketChannelBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  websockets:
    method: POST
    query:
      description: this mah query
    headers:
      description: this mah binding";

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

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Websockets;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }
    }
}
