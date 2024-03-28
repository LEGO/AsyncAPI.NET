// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.Sns
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Sns;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class SnsBindings_Should : TestBase
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
                            new AsyncApiAny(new Dictionary<string, string>()
                            {
                                { "orderingXPropertyName", "orderingXPropertyValue" },
                            })
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
                            Action = new StringOrStringList(new AsyncApiAny(new List<string>()
                            {
                                "sns:Publish",
                                "sns:Delete",
                            })),
                        },
                        new Statement()
                        {
                            Effect = Effect.Allow,
                            Principal = new StringOrStringList(new AsyncApiAny(new List<string>()
                            {
                                "arn:aws:iam::123456789012:user/alex.wichmann",
                                "arn:aws:iam::123456789012:user/dec.kolakowski",
                            })),
                            Action = new StringOrStringList(new AsyncApiAny("sns:Create")),
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-statementExtension",
                                    new AsyncApiAny(new Dictionary<string, string>()
                                    {
                                        { "statementXPropertyName", "statementXPropertyValue" },
                                    })
                                },
                            },
                        },
                    },
                    Extensions = new Dictionary<string, IAsyncApiExtension>()
                    {
                        {
                            "x-policyExtension",
                            new AsyncApiAny(new Dictionary<string, string>()
                            {
                                { "policyXPropertyName", "policyXPropertyValue" },
                            })
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
                        new AsyncApiAny(new Dictionary<string, string>()
                        {
                            { "bindingXPropertyName", "bindingXPropertyValue" },
                        })
                    },
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Sns;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);

            var expectedSnsBinding = (SnsChannelBinding)channel.Bindings.Values.First();
            expectedSnsBinding.Should().BeEquivalentTo((SnsChannelBinding)binding.Bindings.Values.First(), options => options.IgnoringCyclicReferences());
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
                            new AsyncApiAny(new Dictionary<string, string>()
                            {
                                { "identifierXPropertyName", "identifierXPropertyValue" },
                            })
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
                                    new AsyncApiAny(new Dictionary<string, string>()
                                    {
                                        { "identifierXPropertyName", "identifierXPropertyValue" },
                                    })
                                },
                            },
                        },
                        FilterPolicy = new AsyncApiAny(new Dictionary<string, object>()
                        {
                            { "store", new List<string>() { "asyncapi_corp" } },
                            { "contact", "dec.kolakowski" },
                            {
                                "event", new List<Dictionary<string, string>>()
                                {
                                    new Dictionary<string, string>()
                                    {
                                        { "anything-but", "order_cancelled" },
                                    },
                                }
                            },
                            {
                                "order_key", new Dictionary<string, string>()
                                {
                                    { "transient", "by_area" },
                                }
                            },
                            {
                                "customer_interests", new List<string>()
                                {
                                    "rugby",
                                    "football",
                                    "baseball",
                                }
                            },
                        }),
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
                                        new AsyncApiAny(new Dictionary<string, string>()
                                        {
                                            { "identifierXPropertyName", "identifierXPropertyValue" },
                                        })
                                    },
                                },
                            },
                            MaxReceiveCount = 25,
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-redrivePolicyExtension",
                                    new AsyncApiAny(new Dictionary<string, string>()
                                    {
                                        { "redrivePolicyXPropertyName", "redrivePolicyXPropertyValue" },
                                    })
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
                                    new AsyncApiAny(new Dictionary<string, string>()
                                    {
                                        { "deliveryPolicyXPropertyName", "deliveryPolicyXPropertyValue" },
                                    })
                                },
                            },
                        },
                        Extensions = new Dictionary<string, IAsyncApiExtension>()
                        {
                            {
                                "x-consumerExtension",
                                new AsyncApiAny(new Dictionary<string, string>()
                                {
                                    { "consumerXPropertyName", "consumerXPropertyValue" },
                                })
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
                            new AsyncApiAny(new Dictionary<string, string>()
                            {
                                { "deliveryPolicyXPropertyName", "deliveryPolicyXPropertyValue" },
                            })
                        },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-bindingExtension",
                        new AsyncApiAny(new Dictionary<string, string>()
                        {
                            { "bindingXPropertyName", "bindingXPropertyValue" },
                        })
                    },
                },
            });

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Sns;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);
            var binding2 = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(expected, AsyncApiVersion.AsyncApi2_0, out _);
            binding2.Bindings.First().Value.Extensions.TryGetValue("x-bindingExtension", out IAsyncApiExtension any);
            var val = AsyncApiAny.FromExtensionOrDefault<ExtensionClass>(any);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);

            var expectedSnsBinding = (SnsOperationBinding)operation.Bindings.Values.First();
            expectedSnsBinding.Should().BeEquivalentTo((SnsOperationBinding)binding.Bindings.Values.First(), options => options.IgnoringCyclicReferences());
        }

        class ExtensionClass
        {
            public string bindingXPropertyName { get; set; }
        }
    }
}