using System;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Interfaces;
using BindingsCollection = LEGO.AsyncAPI.Bindings.BindingsCollection;

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
                                { "orderingXPropertyName", new AsyncApiString("orderingXPropertyValue") },
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
                            Principal = new StringOrStringList()
                            {
                                StringValue = "arn:aws:iam::123456789012:user/alex.wichmann",
                            },
                            Action = new StringOrStringList()
                            {
                                StringList = new List<string>()
                                {
                                    "sns:Publish",
                                    "sns:Delete",
                                },
                            },
                        },
                        new Statement()
                        {
                            Effect = Effect.Allow,
                            Principal = new StringOrStringList()
                            {
                                StringList = new List<string>()
                                {
                                    "arn:aws:iam::123456789012:user/alex.wichmann",
                                    "arn:aws:iam::123456789012:user/dec.kolakowski",
                                },
                            },
                            Action = new StringOrStringList()
                            {
                                StringValue = "sns:Create",
                            },
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-statementExtension",
                                    new AsyncApiObject()
                                    {
                                        { "statementXPropertyName", new AsyncApiString("statementXPropertyValue") },
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
                                { "policyXPropertyName", new AsyncApiString("policyXPropertyValue") },
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
                            { "bindingXPropertyName", new AsyncApiString("bindingXPropertyValue") },
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
            settings.Bindings.Add(BindingsCollection.Sns);
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
          attributes:
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
          x-filterPolicyExtension:
            filterPolicyXPropertyName: filterPolicyXPropertyValue
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
                                { "identifierXPropertyName", new AsyncApiString("identifierXPropertyValue") },
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
                                        { "identifierXPropertyName", new AsyncApiString("identifierXPropertyValue") },
                                    }
                                },
                            },
                        },
                        FilterPolicy = new FilterPolicy()
                        {
                            Attributes = new AsyncApiObject()
                            {
                                { "store", new AsyncApiArray() { new AsyncApiString("asyncapi_corp") } },
                                { "contact", new AsyncApiString("dec.kolakowski") },
                                {
                                    "event", new AsyncApiArray()
                                    {
                                        new AsyncApiObject()
                                        {
                                            { "anything-but", new AsyncApiString("order_cancelled") },
                                        },
                                    }
                                },
                                {
                                    "order_key", new AsyncApiObject()
                                    {
                                        { "transient", new AsyncApiString("by_area") },
                                    }
                                },
                                {
                                    "customer_interests", new AsyncApiArray()
                                    {
                                        new AsyncApiString("rugby"),
                                        new AsyncApiString("football"),
                                        new AsyncApiString("baseball"),
                                    }
                                },
                            },
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-filterPolicyExtension",
                                    new AsyncApiObject()
                                    {
                                        { "filterPolicyXPropertyName", new AsyncApiString("filterPolicyXPropertyValue") },
                                    }
                                },
                            },
                        },
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
                                            { "identifierXPropertyName", new AsyncApiString("identifierXPropertyValue") },
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
                                        { "redrivePolicyXPropertyName", new AsyncApiString("redrivePolicyXPropertyValue") },
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
                                        { "deliveryPolicyXPropertyName", new AsyncApiString("deliveryPolicyXPropertyValue") },
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
                                    { "consumerXPropertyName", new AsyncApiString("consumerXPropertyValue") },
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
                                { "deliveryPolicyXPropertyName", new AsyncApiString("deliveryPolicyXPropertyValue") },
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
                            { "bindingXPropertyName", new AsyncApiString("bindingXPropertyValue") },
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
            settings.Bindings.Add(BindingsCollection.Sns);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);


            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(operation);

        }
    }
}