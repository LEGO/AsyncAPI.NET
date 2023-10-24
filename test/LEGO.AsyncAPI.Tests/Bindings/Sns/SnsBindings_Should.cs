using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Tests.Bindings.Sns
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Sns;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;

    internal class SnsBindings_Should
    {
        [Test]
        public void SnsChannelBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
                @"bindings:
  sns:
    name: myTopic
    ordering:
      type: FIFO
      contentBasedDeduplication: true
      x-orderingExtension:
        orderingXPropertyName: orderingXPropertyValue
    policy:
      statements:
        - effect: Deny
          principal: arn:aws:iam::123456789012:user/alex.wichmann
          action:
            - sns:Publish
            - sns:Delete
        - effect: Allow
          principal:
            - arn:aws:iam::123456789012:user/alex.wichmann
            - arn:aws:iam::123456789012:user/dec.kolakowski
          action: sns:Create
          x-statementExtension:
            statementXPropertyName: statementXPropertyValue
      x-policyExtension:
        policyXPropertyName: policyXPropertyValue
    tags:
      owner: AsyncAPI.NET
      platform: AsyncAPIOrg
    x-bindingExtension:
      bindingXPropertyName: bindingXPropertyValue";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new SnsChannelBinding()
            {
                Name = "myTopic",
                Ordering = new Ordering()
                {
                    Type = OrderingType.Fifo,
                    ContentBasedDeduplication = true,
                    Extensions = new Dictionary<string, IAsyncApiExtension>()
                    {
                        {
                            "x-orderingExtension",
                            new AsyncApiObject()
                            {
                                { "orderingXPropertyName", new AsyncApiAny("orderingXPropertyValue") },
                            }
                        },
                    },
                },
                Policy = new Policy()
                {
                    Statements = new List<Statement>()
                    {
                        new Statement()
                        {
                            Effect = Effect.Deny,
                            Principal = new StringOrStringList(new AsyncApiAny("arn:aws:iam::123456789012:user/alex.wichmann")),
                            Action = new StringOrStringList(new AsyncApiArray()
                            {
                                new AsyncApiAny("sns:Publish"),
                                new AsyncApiAny("sns:Delete")
                            }),
                        },
                        new Statement()
                        {
                            Effect = Effect.Allow,
                            Principal = new StringOrStringList(new AsyncApiArray()
                            {
                                new AsyncApiAny("arn:aws:iam::123456789012:user/alex.wichmann"),
                                new AsyncApiAny("arn:aws:iam::123456789012:user/dec.kolakowski")
                            }),
                            Action = new StringOrStringList(new AsyncApiAny("sns:Create")),
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-statementExtension",
                                    new AsyncApiObject()
                                    {
                                        { "statementXPropertyName", new AsyncApiAny("statementXPropertyValue") },
                                    }
                                },
                            },
                        },
                    },
                    Extensions = new Dictionary<string, IAsyncApiExtension>()
                    {
                        {
                            "x-policyExtension",
                            new AsyncApiObject()
                            {
                                { "policyXPropertyName", new AsyncApiAny("policyXPropertyValue") },
                            }
                        },
                    },
                },
                Tags = new Dictionary<string, string>()
                {
                    { "owner", "AsyncAPI.NET" },
                    { "platform", "AsyncAPIOrg" },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-bindingExtension",
                        new AsyncApiObject()
                        {
                            { "bindingXPropertyName", new AsyncApiAny("bindingXPropertyValue") },
                        }
                    },
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Sns;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(channel);

        }
        
        [Test]
        public void SnsOperationBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
                @"bindings:
  sns:
    topic:
      name: someTopic
      x-identifierExtension:
        identifierXPropertyName: identifierXPropertyValue
    consumers:
      - protocol: sqs
        endpoint:
          name: someQueue
          x-identifierExtension:
            identifierXPropertyName: identifierXPropertyValue
        filterPolicy:
          store:
            - asyncapi_corp
          contact: dec.kolakowski
          event:
            - anything-but: order_cancelled
          order_key:
            transient: by_area
          customer_interests:
            - rugby
            - football
            - baseball
        filterPolicyScope: MessageAttributes
        rawMessageDelivery: false
        redrivePolicy:
          deadLetterQueue:
            arn: arn:aws:SQS:eu-west-1:0000000:123456789
            x-identifierExtension:
              identifierXPropertyName: identifierXPropertyValue
          maxReceiveCount: 25
          x-redrivePolicyExtension:
            redrivePolicyXPropertyName: redrivePolicyXPropertyValue
        deliveryPolicy:
          minDelayTarget: 10
          maxDelayTarget: 100
          numRetries: 5
          numNoDelayRetries: 2
          numMinDelayRetries: 3
          numMaxDelayRetries: 5
          backoffFunction: linear
          maxReceivesPerSecond: 2
          x-deliveryPolicyExtension:
            deliveryPolicyXPropertyName: deliveryPolicyXPropertyValue
        x-consumerExtension:
          consumerXPropertyName: consumerXPropertyValue
    deliveryPolicy:
      minDelayTarget: 10
      maxDelayTarget: 100
      numRetries: 5
      numNoDelayRetries: 2
      numMinDelayRetries: 3
      numMaxDelayRetries: 5
      backoffFunction: geometric
      maxReceivesPerSecond: 10
      x-deliveryPolicyExtension:
        deliveryPolicyXPropertyName: deliveryPolicyXPropertyValue
    x-bindingExtension:
      bindingXPropertyName: bindingXPropertyValue";

            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new SnsOperationBinding()
            {
                Topic = new Identifier()
                {
                    Name = "someTopic",
                    Extensions = new Dictionary<string, IAsyncApiExtension>()
                    {
                        {
                            "x-identifierExtension",
                            new AsyncApiObject()
                            {
                                { "identifierXPropertyName", new AsyncApiAny("identifierXPropertyValue") },
                            }
                        },
                    },
                },
                Consumers = new List<Consumer>()
                {
                    new Consumer()
                    {
                        Protocol = Protocol.Sqs,
                        Endpoint = new Identifier()
                        {
                            Name = "someQueue",
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-identifierExtension",
                                    new AsyncApiObject()
                                    {
                                        { "identifierXPropertyName", new AsyncApiAny("identifierXPropertyValue") },
                                    }
                                },
                            },
                        },
                        FilterPolicy = new AsyncApiObject()
                        { 
                            { "store", new AsyncApiArray() { new AsyncApiAny("asyncapi_corp") } },
                            { "contact", new AsyncApiAny("dec.kolakowski") },
                            {
                                "event", new AsyncApiArray()
                                {
                                    new AsyncApiObject()
                                    {
                                        { "anything-but", new AsyncApiAny("order_cancelled") },
                                    },
                                }
                            },
                            {
                                "order_key", new AsyncApiObject()
                                {
                                    { "transient", new AsyncApiAny("by_area") },
                                }
                            },
                            {
                                "customer_interests", new AsyncApiArray()
                                {
                                    new AsyncApiAny("rugby"),
                                    new AsyncApiAny("football"),
                                    new AsyncApiAny("baseball"),
                                }
                            },
                        },
                        FilterPolicyScope = FilterPolicyScope.MessageAttributes,
                        RawMessageDelivery = false,
                        RedrivePolicy = new RedrivePolicy()
                        {
                            DeadLetterQueue = new Identifier()
                            {
                                Arn = "arn:aws:SQS:eu-west-1:0000000:123456789",
                                Extensions = new Dictionary<string, IAsyncApiExtension>()
                                {
                                    {
                                        "x-identifierExtension",
                                        new AsyncApiObject()
                                        {
                                            { "identifierXPropertyName", new AsyncApiAny("identifierXPropertyValue") },
                                        }
                                    },
                                },
                            },
                            MaxReceiveCount = 25,
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-redrivePolicyExtension",
                                    new AsyncApiObject()
                                    {
                                        { "redrivePolicyXPropertyName", new AsyncApiAny("redrivePolicyXPropertyValue") },
                                    }
                                },
                            },
                        },
                        DeliveryPolicy = new DeliveryPolicy()
                        {
                            MinDelayTarget = 10,
                            MaxDelayTarget = 100,
                            NumRetries = 5,
                            NumNoDelayRetries = 2,
                            NumMinDelayRetries = 3,
                            NumMaxDelayRetries = 5,
                            BackoffFunction = BackoffFunction.Linear,
                            MaxReceivesPerSecond = 2,
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-deliveryPolicyExtension",
                                    new AsyncApiObject()
                                    {
                                        { "deliveryPolicyXPropertyName", new AsyncApiAny("deliveryPolicyXPropertyValue") },
                                    }
                                },
                            },
                        },
                        Extensions = new Dictionary<string, IAsyncApiExtension>()
                        {
                            {
                                "x-consumerExtension",
                                new AsyncApiObject()
                                {
                                    { "consumerXPropertyName", new AsyncApiAny("consumerXPropertyValue") },
                                }
                            },
                        },
                    },
                },
                DeliveryPolicy = new DeliveryPolicy()
                {
                    MinDelayTarget = 10,
                    MaxDelayTarget = 100,
                    NumRetries = 5,
                    NumNoDelayRetries = 2,
                    NumMinDelayRetries = 3,
                    NumMaxDelayRetries = 5,
                    BackoffFunction = BackoffFunction.Geometric,
                    MaxReceivesPerSecond = 10,
                    Extensions = new Dictionary<string, IAsyncApiExtension>()
                    {
                        {
                            "x-deliveryPolicyExtension",
                            new AsyncApiObject()
                            {
                                { "deliveryPolicyXPropertyName", new AsyncApiAny("deliveryPolicyXPropertyValue") },
                            }
                        },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-bindingExtension",
                        new AsyncApiObject()
                        {
                            { "bindingXPropertyName", new AsyncApiAny("bindingXPropertyValue") },
                        }
                    },
                },
            });

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Sns;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);


            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(operation);

        }
    }
}