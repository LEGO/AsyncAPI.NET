// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using NUnit.Framework;

    internal class AsyncApiServer_Should : TestBase
    {
        [Test]
        public void AsyncApiServer_Serializes()
        {
            // Arrange
            var expected =
@"url: 'https://example.com/{channelkey}'
protocol: test
protocolVersion: 0.1.0
description: some description
variables:
  channelkey:
    description: some description
security:
  - schem1:
      - requirement
tags:
  - name: mytag1
    description: description of tag1
bindings:
  kafka:
    schemaRegistryUrl: http://example.com
    schemaRegistryVendor: kafka";

            var server = new AsyncApiServer
            {
                Url = "https://example.com/{channelkey}",
                Protocol = "test",
                ProtocolVersion = "0.1.0",
                Description = "some description",
            };
            server.Variables.Add("channelkey", new AsyncApiServerVariable { Description = "some description" });
            server.Security.Add(
                new AsyncApiSecurityRequirement
                {
                    {
                        new AsyncApiSecurityScheme()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "schem1",
                                Type = ReferenceType.SecurityScheme,
                            },
                            Name = "scheme1",
                        }, new List<string>
                        {
                            "requirement",
                        }
                    },
                });
            server.Tags.Add(new AsyncApiTag { Name = "mytag1", Description = "description of tag1" });
            server.Bindings.Add(new KafkaServerBinding
            {
                SchemaRegistryUrl = "http://example.com",
                SchemaRegistryVendor = "kafka",
            });

            // Act
            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void AsyncApiServer_WithKafkaBinding_Serializes()
        {
            var expected =
@"url: 
protocol: 
bindings:
  kafka:
    schemaRegistryUrl: http://example.com
    schemaRegistryVendor: kafka";
            var server = new AsyncApiServer
            {
                Bindings = new AsyncApiBindings<IServerBinding>
                {
                    {
                        new KafkaServerBinding
                        {
                            SchemaRegistryUrl = "http://example.com",
                            SchemaRegistryVendor = "kafka",
                        }
                    },
                },
            };

            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                 .BePlatformAgnosticEquivalentTo(expected);
        }
    }
}
