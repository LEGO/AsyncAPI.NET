// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Bindings.WebSockets;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using NUnit.Framework;

    internal class AsyncApiChannel_Should
    {
        [Test]
        public void AsyncApiChannel_WithWebSocketsBinding_Serializes()
        {
            var expected = @"bindings:
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
    bindingVersion: 0.1.0";

            var channel = new AsyncApiChannel
            {
                Bindings = new AsyncApiBindings<IChannelBinding>
                {
                    {
                        new WebSocketsChannelBinding()
                        {
                            Method = "POST",
                            Query = new JsonSchema()
                            {
                                Properties = new Dictionary<string, JsonSchema>()
                                {
                                    {
                                        "index", new JsonSchema()
                                        {
                                            Description = "the index",
                                        }
                                    },
                                },
                            },
                            Headers = new JsonSchema()
                            {
                                Properties = new Dictionary<string, JsonSchema>()
                                {
                                    {
                                        "x-correlation-id", new JsonSchema()
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

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AsyncApiChannel_WithKafkaBinding_Serializes()
        {
            var expected =
@"bindings:
  kafka:
    topic: topic
    partitions: 5
    replicas: 2";

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

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
