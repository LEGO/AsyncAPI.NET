// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using System.IO;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings.Http;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class AsyncApiOperation_Should : TestBase
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiOperation = new AsyncApiOperation();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiOperation.SerializeV2(null); });
        }

        [Test]
        public void SerializeV2_WithMultipleMessages_SerializesWithOneOf()
        {
            // Arrange
            var expected = """
                message:
                  oneOf:
                    - name: First Message
                    - name: Second Message
                """;

            var asyncApiOperation = new AsyncApiOperation();
            asyncApiOperation.Message.Add(new AsyncApiMessage { Name = "First Message" });
            asyncApiOperation.Message.Add(new AsyncApiMessage { Name = "Second Message" });
            var outputString = new StringWriter();
            var settings = new AsyncApiWriterSettings();
            var writer = new AsyncApiYamlWriter(outputString, settings);

            // Act
            asyncApiOperation.SerializeV2(writer);

            // Assert
            var actual = outputString.GetStringBuilder().ToString();

            actual.Should()
                .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void SerializeV2_WithSingleMessage_Serializes()
        {
            // Arrange
            var expected = """
                message:
                  name: First Message
                """;

            var asyncApiOperation = new AsyncApiOperation();
            asyncApiOperation.Message.Add(new AsyncApiMessage { Name = "First Message" });
            var settings = new AsyncApiWriterSettings();
            var outputString = new StringWriter();
            var writer = new AsyncApiYamlWriter(outputString, settings);

            // Act
            asyncApiOperation.SerializeV2(writer);

            // Assert
            var actual = outputString.GetStringBuilder().ToString();

            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiOperation_WithBindings_Serializes()
        {
            var expected =
                """
                bindings:
                  http:
                    type: request
                    method: PUT
                    query:
                      description: some query
                  kafka:
                    groupId:
                      description: some Id
                    clientId:
                      description: some Id
                """;

            var operation = new AsyncApiOperation
            {
                Bindings = new AsyncApiBindings<IOperationBinding>
                {
                    {
                        new HttpOperationBinding
                        {
                            Type = HttpOperationBinding.HttpOperationType.Request,
                            Method = "PUT",
                            Query = new AsyncApiJsonSchema
                            {
                                Description = "some query",
                            },
                        }
                    },
                    {
                        new KafkaOperationBinding
                        {
                            GroupId = new AsyncApiJsonSchema
                            {
                                Description = "some Id",
                            },
                            ClientId = new AsyncApiJsonSchema
                            {
                                Description = "some Id",
                            },
                        }
                    },
                },
            };

            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                .BePlatformAgnosticEquivalentTo(expected);
        }
    }
}
