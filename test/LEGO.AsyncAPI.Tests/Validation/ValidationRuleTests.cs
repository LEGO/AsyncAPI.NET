// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Validation
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;
    using System.Linq;

    public class ValidationRuleTests
    {
        [Test]
        [TestCase("chat-{person-id}")]
        public void ChannelKey_WithInvalidParameter_DiagnosticsError(string channelKey)
        {
            var input =
                $"""
                asyncapi: 2.6.0
                info:
                  title: Chat Application
                  version: 1.0.0
                servers:
                  testing:
                    url: test.mosquitto.org:1883
                    protocol: mqtt
                    description: Test broker
                channels:
                  {channelKey}:
                    publish:
                      operationId: onMessageReceieved
                      message:
                        name: text
                        payload:
                          type: string
                    subscribe:
                      operationId: sendMessage
                      message:
                        name: text
                        payload:
                          type: string
                """;

            var document = new AsyncApiStringReader().Read(input, out var diagnostic);
            diagnostic.Errors.First().Message.Should().Be($"The key '{channelKey}' in 'channels' MUST match the regular expression '^[a-zA-Z0-9\\.\\-_]+$'.");
            diagnostic.Errors.First().Pointer.Should().Be("#/channels");
        }

        [Test]
        public void ChannelKey_WithNonUniqueKey_DiagnosticsError()
        {
            var input =
                """
                asyncapi: 2.6.0
                info:
                  title: Chat Application
                  version: 1.0.0
                servers:
                  testing:
                    url: test.mosquitto.org:1883
                    protocol: mqtt
                    description: Test broker
                channels:
                  chat/{personId}:
                    publish:
                      operationId: onMessageReceieved
                      message:
                        name: text
                        payload:
                          type: string
                  chat/{personIdentity}:
                    publish:
                      operationId: onMessageReceieved
                      message:
                        name: text
                        payload:
                          type: string
                """;

            var document = new AsyncApiStringReader().Read(input, out var diagnostic);
            diagnostic.Errors.First().Message.Should().Be("Channel signature 'chat/{}' MUST be unique.");
            diagnostic.Errors.First().Pointer.Should().Be("#/channels");
        }

        [Test]
        [TestCase("chat")]
        [TestCase("/some/chat/{personId}")]
        [TestCase("chat-{personId}")]
        [TestCase("chat-{person_id}")]
        [TestCase("chat-{person%2Did}")]
        [TestCase("chat-{personId2}")]
        public void ChannelKey_WithValidKey_Success(string channelKey)
        {
            var input =
                $"""
                asyncapi: 2.6.0
                info:
                  title: Chat Application
                  version: 1.0.0
                servers:
                  testing:
                    url: test.mosquitto.org:1883
                    protocol: mqtt
                    description: Test broker
                channels:
                  {channelKey}:
                    publish:
                      operationId: onMessageReceieved
                      message:
                        name: text
                        payload:
                          type: string
                    subscribe:
                      operationId: sendMessage
                      message:
                        name: text
                        payload:
                          type: string
                """;

            var document = new AsyncApiStringReader().Read(input, out var diagnostic);
            diagnostic.Errors.Should().BeEmpty();
        }
    }

}
