using System;
using LEGO.AsyncAPI.Models.Any;
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
    policy:
      statements:
        - effect: deny
          principal: arn:aws:iam::123456789012:user/alex.wichmann
          action:
            - sns:Publish
            - sns:Delete
        - effect: allow
          principal:
            - arn:aws:iam::123456789012:user/alex.wichmann
            - arn:aws:iam::123456789012:user/dec.kolakowski
          action: sns:Create
    tags:
      owner: AsyncAPI.NET
      platform: AsyncAPIOrg";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new SnsChannelBinding()
            {
                Name = "myTopic",
                Ordering = new Ordering()
                {
                    Type = OrderingType.Fifo,
                    ContentBasedDeduplication = true,
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
                        },
                    },
                },
                Tags = new Dictionary<string, string>()
                {
                    { "owner", "AsyncAPI.NET" },
                    { "platform", "AsyncAPIOrg" },
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
    consumers:
      - protocol: sqs
        endpoint:
          name: someQueue
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
        rawMessageDelivery: false
        redrivePolicy:
          deadLetterQueue:
            arn: arn:aws:SQS:eu-west-1:0000000:123456789
          maxReceiveCount: 25
        deliveryPolicy:
          minDelayTarget: 10
          maxDelayTarget: 100
          numRetries: 5
          numNoDelayRetries: 2
          numMinDelayRetries: 3
          numMaxDelayRetries: 5
          backoffFunction: linear
          maxReceivesPerSecond: 2
    deliveryPolicy:
      minDelayTarget: 10
      maxDelayTarget: 100
      numRetries: 5
      numNoDelayRetries: 2
      numMinDelayRetries: 3
      numMaxDelayRetries: 5
      backoffFunction: geometric
      maxReceivesPerSecond: 10";

            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new SnsOperationBinding()
            {
                Topic = new Identifier()
                {
                    Name = "someTopic",
                },
                Consumers = new List<Consumer>()
                {
                    new Consumer()
                    {
                        Protocol = Protocol.Sqs,
                        Endpoint = new Identifier()
                        {
                            Name = "someQueue",
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
                        },
                        RawMessageDelivery = false,
                        RedrivePolicy = new RedrivePolicy()
                        {
                            DeadLetterQueue = new Identifier()
                            {
                                Arn = "arn:aws:SQS:eu-west-1:0000000:123456789"
                            },
                            MaxReceiveCount = 25,
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