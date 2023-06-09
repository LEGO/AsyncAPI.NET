namespace LEGO.AsyncAPI.Tests.Bindings.Sqs
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Sqs;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;
    using BindingsCollection = LEGO.AsyncAPI.Bindings.BindingsCollection;

    internal class SqsBindings_should
    {
        [Test]
        public void SqsChannelBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
                @"bindings:
  sqs:
    queue:
      name: myQueue
      fifoQueue: true
      deliveryDelay: 30
      visibilityTimeout: 60
      receiveMessageWaitTime: 0
      messageRetentionPeriod: 86400
      redrivePolicy:
        deadLetterQueue:
          arn: arn:aws:SQS:eu-west-1:0000000:123456789
          x-identifierExtension:
            identifierXPropertyName: identifierXPropertyValue
        maxReceiveCount: 15
        x-redrivePolicyExtension:
          redrivePolicyXPropertyName: redrivePolicyXPropertyValue
      policy:
        statements:
          - effect: deny
            principal: arn:aws:iam::123456789012:user/alex.wichmann
            action:
              - sqs:SendMessage
              - sqs:ReceiveMessage
            x-statementExtension:
              statementXPropertyName: statementXPropertyValue
          - effect: allow
            principal:
              - arn:aws:iam::123456789012:user/alex.wichmann
              - arn:aws:iam::123456789012:user/dec.kolakowski
            action: sqs:CreateQueue
        x-policyExtension:
          policyXPropertyName: policyXPropertyValue
      tags:
        owner: AsyncAPI.NET
        platform: AsyncAPIOrg
      x-queueExtension:
        queueXPropertyName: queueXPropertyValue
    deadLetterQueue:
      name: myQueue_error
      deliveryDelay: 0
      visibilityTimeout: 0
      receiveMessageWaitTime: 0
      messageRetentionPeriod: 604800
      policy:
        statements:
          - effect: allow
            principal: arn:aws:iam::123456789012:user/alex.wichmann
            action:
              - sqs:*
    x-internalObject:
      myExtensionPropertyName: myExtensionPropertyValue";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new SqsChannelBinding()
            {
                Queue = new Queue()
                {
                    Name = "myQueue",
                    FifoQueue = true,
                    DeliveryDelay = 30,
                    VisibilityTimeout = 60,
                    ReceiveMessageWaitTime = 0,
                    MessageRetentionPeriod = 86400,
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
                        MaxReceiveCount = 15,
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
                    Policy = new Policy()
                    {
                        Statements = new List<Statement>()
                        {
                            new Statement()
                            {
                                Effect = Effect.Deny,
                                Principal = new StringOrStringList(new AsyncApiString("arn:aws:iam::123456789012:user/alex.wichmann")),
                                Action = new StringOrStringList(new AsyncApiArray()
                                {
                                    new AsyncApiString("sqs:SendMessage"),
                                    new AsyncApiString("sqs:ReceiveMessage")
                                }),
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
                            new Statement()
                            {
                                Effect = Effect.Allow,
                                Principal = new StringOrStringList(new AsyncApiArray
                                {
                                        new AsyncApiString("arn:aws:iam::123456789012:user/alex.wichmann"),
                                        new AsyncApiString("arn:aws:iam::123456789012:user/dec.kolakowski")
                                }),
                                Action = new StringOrStringList(new AsyncApiString("sqs:CreateQueue")),
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
                            "x-queueExtension",
                            new AsyncApiObject()
                            {
                                { "queueXPropertyName", new AsyncApiString("queueXPropertyValue") },
                            }
                        },
                    },
                },
                DeadLetterQueue = new Queue()
                {
                    Name = "myQueue_error",
                    FifoQueue = false,
                    DeliveryDelay = 0,
                    VisibilityTimeout = 0,
                    ReceiveMessageWaitTime = 0,
                    MessageRetentionPeriod = 604800,
                    Policy = new Policy()
                    {
                        Statements = new List<Statement>()
                        {
                            new Statement()
                            {
                                Effect = Effect.Allow,
                                Principal = new StringOrStringList(new AsyncApiString("arn:aws:iam::123456789012:user/alex.wichmann")),
                                Action = new StringOrStringList(new AsyncApiArray()
                                {
                                    new AsyncApiString("sqs:*")
                                })
                            },
                        },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-internalObject", new AsyncApiObject()
                        {
                            { "myExtensionPropertyName", new AsyncApiString("myExtensionPropertyValue") },
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
            settings.Bindings.Add(BindingsCollection.Sqs);
            var binding =
                new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0,
                    out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }

        [Test]
        public void SqsOperationBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
                @"bindings:
  sqs:
    queues:
      - name: myQueue
        fifoQueue: true
        deliveryDelay: 30
        visibilityTimeout: 60
        receiveMessageWaitTime: 0
        messageRetentionPeriod: 86400
        redrivePolicy:
          deadLetterQueue:
            arn: arn:aws:SQS:eu-west-1:0000000:123456789
            x-identifierExtension:
              identifierXPropertyName: identifierXPropertyValue
          maxReceiveCount: 15
          x-redrivePolicyExtension:
            redrivePolicyXPropertyName: redrivePolicyXPropertyValue
        policy:
          statements:
            - effect: deny
              principal: arn:aws:iam::123456789012:user/alex.wichmann
              action:
                - sqs:SendMessage
                - sqs:ReceiveMessage
              x-statementExtension:
                statementXPropertyName: statementXPropertyValue
            - effect: allow
              principal:
                - arn:aws:iam::123456789012:user/alex.wichmann
                - arn:aws:iam::123456789012:user/dec.kolakowski
              action: sqs:CreateQueue
          x-policyExtension:
            policyXPropertyName: policyXPropertyValue
        tags:
          owner: AsyncAPI.NET
          platform: AsyncAPIOrg
        x-queueExtension:
          queueXPropertyName: queueXPropertyValue
      - name: myQueue_error
        deliveryDelay: 0
        visibilityTimeout: 0
        receiveMessageWaitTime: 0
        messageRetentionPeriod: 604800
        policy:
          statements:
            - effect: allow
              principal: arn:aws:iam::123456789012:user/alex.wichmann
              action:
                - sqs:*
        x-queueExtension:
          queueXPropertyName: queueXPropertyValue
    x-internalObject:
      myExtensionPropertyName: myExtensionPropertyValue";

            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new SqsOperationBinding()
            {
                Queues = new List<Queue>()
                {
                    new Queue()
                    {
                        Name = "myQueue",
                        FifoQueue = true,
                        DeliveryDelay = 30,
                        VisibilityTimeout = 60,
                        ReceiveMessageWaitTime = 0,
                        MessageRetentionPeriod = 86400,
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
                            MaxReceiveCount = 15,
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
                        Policy = new Policy()
                        {
                            Statements = new List<Statement>()
                            {
                                new Statement()
                                {
                                    Effect = Effect.Deny,
                                    Principal = new StringOrStringList(new AsyncApiString("arn:aws:iam::123456789012:user/alex.wichmann")),
                                    Action = new StringOrStringList(new AsyncApiArray()
                                    {
                                        new AsyncApiString("sqs:SendMessage"),
                                        new AsyncApiString("sqs:ReceiveMessage")
                                    }),
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
                                new Statement()
                                {
                                    Effect = Effect.Allow,
                                    Principal = new StringOrStringList(new AsyncApiArray
                                    {
                                        new AsyncApiString("arn:aws:iam::123456789012:user/alex.wichmann"),
                                        new AsyncApiString("arn:aws:iam::123456789012:user/dec.kolakowski"),
                                    }),
                                    Action = new StringOrStringList(new AsyncApiString("sqs:CreateQueue"))
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
                                "x-queueExtension",
                                new AsyncApiObject()
                                {
                                    { "queueXPropertyName", new AsyncApiString("queueXPropertyValue") },
                                }
                            },
                        },
                    },
                    new Queue()
                    {
                        Name = "myQueue_error",
                        FifoQueue = false,
                        DeliveryDelay = 0,
                        VisibilityTimeout = 0,
                        ReceiveMessageWaitTime = 0,
                        MessageRetentionPeriod = 604800,
                        Policy = new Policy()
                        {
                            Statements = new List<Statement>()
                            {
                                new Statement()
                                {
                                    Effect = Effect.Allow,
                                    Principal = new StringOrStringList(new AsyncApiString("arn:aws:iam::123456789012:user/alex.wichmann")),
                                    Action = new StringOrStringList(new AsyncApiArray
                                    {
                                        new AsyncApiString("sqs:*")
                                    })
                                },
                            },
                        },
                        Extensions = new Dictionary<string, IAsyncApiExtension>()
                        {
                            {
                                "x-queueExtension",
                                new AsyncApiObject()
                                {
                                    { "queueXPropertyName", new AsyncApiString("queueXPropertyValue") },
                                }
                            },
                        },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-internalObject", new AsyncApiObject()
                        {
                            { "myExtensionPropertyName", new AsyncApiString("myExtensionPropertyValue") },
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
            settings.Bindings.Add(BindingsCollection.Sqs);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(operation);
        }
    }
}