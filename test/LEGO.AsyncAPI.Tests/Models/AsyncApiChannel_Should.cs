// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Bindings.WebSockets;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    public class AsyncApiChannel_Should : TestBase
    {
        [Test]
        public void AsyncApiChannel_WithInlineParameter_DoesNotCreateReference()
        {
            var input =
                """
                    parameters:
                      id:
                        description: ids
                        schema:
                          type: string
                          enum:
                            - 08735ae0-6a1a-4578-8b4a-35aa26d15993
                            - 97845c62-329c-4d87-ad24-4f611b909a10
                """;

            var channel = new AsyncApiStringReader().ReadFragment<AsyncApiChannel>(input, AsyncApiVersion.AsyncApi2_0, out var _);
            channel.Parameters.First().Value.Should().NotBeOfType<AsyncApiParameterReference>();
        }

        [Test]
        public void AsyncApiChannel_WithWebSocketsBinding_Serializes()
        {
            var expected = """
                bindings:
                  websockets:
                    method: POST
                    query:
                      properties:
                        index:
                          description: the index
                    headers:
                      properties:
                        x-correlation-id:
                          description: the correlationid
                    bindingVersion: 0.1.0
                """;

            var channel = new AsyncApiChannel
            {
                Bindings = new AsyncApiBindings<IChannelBinding>
                {
                    {
                        new WebSocketsChannelBinding()
                        {
                            Method = "POST",
                            Query = new AsyncApiJsonSchema()
                            {
                                Properties = new Dictionary<string, AsyncApiJsonSchema>()
                                {
                                    {
                                        "index", new AsyncApiJsonSchema()
                                        {
                                            Description = "the index",
                                        }
                                    },
                                },
                            },
                            Headers = new AsyncApiJsonSchema()
                            {
                                Properties = new Dictionary<string, AsyncApiJsonSchema>()
                                {
                                    {
                                        "x-correlation-id", new AsyncApiJsonSchema()
                                        {
                                            Description = "the correlationid",
                                        }
                                    },
                                },
                            },
                            BindingVersion = "0.1.0",
                        }
                    },
                },
            };

            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiChannel_WithKafkaBinding_Serializes()
        {
            var expected =
                """
                bindings:
                  kafka:
                    topic: topic
                    partitions: 5
                    replicas: 2
                """;

            var channel = new AsyncApiChannel
            {
                Bindings = new AsyncApiBindings<IChannelBinding>
                {
                    {
                        new KafkaChannelBinding
                        {
                            Topic = "topic",
                            Partitions = 5,
                            Replicas = 2,
                        }
                    },
                },
            };

            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }
    }
}
