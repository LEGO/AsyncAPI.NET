using System;
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
    type:
      type: FIFO
      contentBasedDeduplication: true
    policy:
      statements:
        - effect: deny
          principal: someARN
          action:
            - sns:Publish
            - sns:Delete
    tags:
      owner: AsyncAPI.NET
      platform: AsyncAPIOrg";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new SnsChannelBinding()
            {
                Name = "myTopic",
                Type = new OrderingConfiguration()
                {
                    Type = Ordering.Fifo,
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
                                StringValue = "someARN",
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
    }
}