// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings.Sqs
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Sqs;
    using LEGO.AsyncAPI.Models;
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
                """
                bindings:
                  sqs:
                    queue:
                      name: myQueue
                      fifoQueue: true
                      deduplicationScope: messageGroup
                      fifoThroughputLimit: perMessageGroupId
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
                            principal:
                              AWS: arn:aws:iam::123456789012:user/alex.wichmann
                            action:
                              - sqs:SendMessage
                              - sqs:ReceiveMessage
                            condition:
                              StringEquals:
                                aws:username:
                                  - johndoe
                                  - mrsmith
                            x-statementExtension:
                              statementXPropertyName: statementXPropertyValue
                          - effect: allow
                            principal:
                              AWS:
                                - arn:aws:iam::123456789012:user/alex.wichmann
                                - arn:aws:iam::123456789012:user/dec.kolakowski
                            action: sqs:CreateQueue
                            condition:
                              NumericLessThanEquals:
                                aws:MultiFactorAuthAge: '3600'
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
                            principal:
                              Service: s3.amazonaws.com
                            action:
                              - sqs:*
                    x-internalObject:
                      myExtensionPropertyName: myExtensionPropertyValue
                """;

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new SqsChannelBinding()
            {
                Queue = new Queue()
                {
                    Name = "myQueue",
                    FifoQueue = true,
                    DeduplicationScope = DeduplicationScope.MessageGroup,
                    FifoThroughputLimit = FifoThroughputLimit.PerMessageGroupId,
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
                                    new AsyncApiAny(new Dictionary<string, string>
                                    {
                                        { "identifierXPropertyName", "identifierXPropertyValue" },
                                    })
                                },
                            },
                        },
                        MaxReceiveCount = 15,
                        Extensions = new Dictionary<string, IAsyncApiExtension>()
                        {
                            {
                                "x-redrivePolicyExtension",
                                new AsyncApiAny(new Dictionary<string, string>
                                {
                                    { "redrivePolicyXPropertyName", "redrivePolicyXPropertyValue" },
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
                                Principal = new PrincipalObject(new KeyValuePair<string, StringOrStringList>(
                                    "AWS", new StringOrStringList(new AsyncApiAny("arn:aws:iam::123456789012:user/alex.wichmann")))),
                                Action = new StringOrStringList(new AsyncApiAny(new List<string>
                                {
                                    "sqs:SendMessage",
                                    "sqs:ReceiveMessage",
                                })),
                                Condition = new Condition(new Dictionary<string, Dictionary<string, StringOrStringList>>
                                {
                                    {
                                        "StringEquals", new Dictionary<string, StringOrStringList>
                                        {
                                            {
                                                "aws:username", new StringOrStringList(new AsyncApiAny(new List<string> { "johndoe", "mrsmith" }))
                                            },
                                        }
                                    },
                                }),
                                Extensions = new Dictionary<string, IAsyncApiExtension>()
                                {
                                    {
                                        "x-statementExtension",
                                        new AsyncApiAny(new Dictionary<string, string>
                                        {
                                            { "statementXPropertyName", "statementXPropertyValue" },
                                        })
                                    },
                                },
                            },
                            new Statement()
                            {
                                Effect = Effect.Allow,
                                Principal = new PrincipalObject(new KeyValuePair<string, StringOrStringList>(
                                    "AWS", new StringOrStringList(new AsyncApiAny(new List<string>
                                        { "arn:aws:iam::123456789012:user/alex.wichmann", "arn:aws:iam::123456789012:user/dec.kolakowski" })))),
                                Action = new StringOrStringList(new AsyncApiAny("sqs:CreateQueue")),
                                Condition = new Condition(new Dictionary<string, Dictionary<string, StringOrStringList>>
                                {
                                    {
                                        "NumericLessThanEquals", new Dictionary<string, StringOrStringList>
                                        {
                                            {
                                                "aws:MultiFactorAuthAge", new StringOrStringList(new AsyncApiAny("3600"))
                                            },
                                        }
                                    },
                                }),
                            },
                        },
                        Extensions = new Dictionary<string, IAsyncApiExtension>()
                        {
                            {
                                "x-policyExtension",
                                new AsyncApiAny(new Dictionary<string, string>
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
                            "x-queueExtension",
                            new AsyncApiAny(new Dictionary<string, string>
                            {
                                { "queueXPropertyName", "queueXPropertyValue" },
                            })
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
                                Principal = new PrincipalObject(new KeyValuePair<string, StringOrStringList>(
                                    "Service", new StringOrStringList(new AsyncApiAny("s3.amazonaws.com")))),
                                Action = new StringOrStringList(new AsyncApiAny(new List<string>
                                {
                                    "sqs:*",
                                })),
                            },
                        },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-internalObject", new AsyncApiAny(new Dictionary<string, string>
                        {
                            { "myExtensionPropertyName", "myExtensionPropertyValue" },
                        })
                    },
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            var settings = new AsyncApiReaderSettings
            {
                Bindings = BindingsCollection.Sqs,
            };
            var binding =
                new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            binding.Should().BeEquivalentTo(channel);

            var expectedSqsBinding = (SqsChannelBinding)channel.Bindings.Values.First();
            expectedSqsBinding.Should().BeEquivalentTo((SqsChannelBinding)binding.Bindings.Values.First(), options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void SqsOperationBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
                """
                bindings:
                  sqs:
                    queues:
                      - name: myQueue
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
                              principal:
                                AWS: arn:aws:iam::123456789012:user/alex.wichmann
                              action:
                                - sqs:SendMessage
                                - sqs:ReceiveMessage
                              x-statementExtension:
                                statementXPropertyName: statementXPropertyValue
                            - effect: allow
                              principal:
                                AWS:
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
                              principal:
                                AWS: arn:aws:iam::123456789012:user/alex.wichmann
                              action:
                                - sqs:*
                        x-queueExtension:
                          queueXPropertyName: queueXPropertyValue
                    x-internalObject:
                      myExtensionPropertyName: myExtensionPropertyValue
                """;

            var operation = new AsyncApiOperation();
            operation.Bindings.Add(new SqsOperationBinding()
            {
                Queues = new List<Queue>()
                {
                    new Queue()
                    {
                        Name = "myQueue",
                        FifoQueue = false,
                        DeduplicationScope = null,
                        FifoThroughputLimit = null,
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
                                        new AsyncApiAny(new Dictionary<string, string>
                                        {
                                            { "identifierXPropertyName", "identifierXPropertyValue" },
                                        })
                                    },
                                },
                            },
                            MaxReceiveCount = 15,
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-redrivePolicyExtension",
                                    new AsyncApiAny(new Dictionary<string, string>
                                    {
                                        { "redrivePolicyXPropertyName", "redrivePolicyXPropertyValue" },
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
                                    Principal = new PrincipalObject(new KeyValuePair<string, StringOrStringList>(
                                        "AWS", new StringOrStringList(new AsyncApiAny("arn:aws:iam::123456789012:user/alex.wichmann")))),
                                    Action = new StringOrStringList(new AsyncApiAny(new List<string>()
                                    {
                                        "sqs:SendMessage",
                                        "sqs:ReceiveMessage",
                                    })),
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
                                new Statement()
                                {
                                    Effect = Effect.Allow,
                                    Principal = new PrincipalObject(new KeyValuePair<string, StringOrStringList>(
                                        "AWS", new StringOrStringList(new AsyncApiAny(new List<string>
                                            { "arn:aws:iam::123456789012:user/alex.wichmann", "arn:aws:iam::123456789012:user/dec.kolakowski" })))),
                                    Action = new StringOrStringList(new AsyncApiAny("sqs:CreateQueue")),
                                },
                            },
                            Extensions = new Dictionary<string, IAsyncApiExtension>()
                            {
                                {
                                    "x-policyExtension",
                                    new AsyncApiAny(new Dictionary<string, string>
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
                                "x-queueExtension",
                                new AsyncApiAny(new Dictionary<string, string>()
                                {
                                    { "queueXPropertyName", "queueXPropertyValue" },
                                })
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
                                    Principal = new PrincipalObject(new KeyValuePair<string, StringOrStringList>(
                                        "AWS", new StringOrStringList(new AsyncApiAny("arn:aws:iam::123456789012:user/alex.wichmann")))),
                                    Action = new StringOrStringList(new AsyncApiAny(new List<string>
                                    {
                                        "sqs:*",
                                    })),
                                },
                            },
                        },
                        Extensions = new Dictionary<string, IAsyncApiExtension>()
                        {
                            {
                                "x-queueExtension",
                                new AsyncApiAny(new Dictionary<string, string>()
                                {
                                    { "queueXPropertyName", "queueXPropertyValue" },
                                })
                            },
                        },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    {
                        "x-internalObject", new AsyncApiAny(new Dictionary<string, string>()
                        {
                            { "myExtensionPropertyName", "myExtensionPropertyValue" },
                        })
                    },
                },
            });

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            var settings = new AsyncApiReaderSettings
            {
                Bindings = BindingsCollection.Sqs,
            };

            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            binding.Should().BeEquivalentTo(operation);

            var expectedSqsBinding = (SqsOperationBinding)operation.Bindings.Values.First();
            expectedSqsBinding.Should().BeEquivalentTo((SqsOperationBinding)binding.Bindings.Values.First(), options => options.IgnoringCyclicReferences());
        }
    }
}