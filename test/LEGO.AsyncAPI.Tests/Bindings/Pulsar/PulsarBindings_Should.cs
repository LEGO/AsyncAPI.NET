namespace LEGO.AsyncAPI.Tests.Bindings.Pulsar
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.Pulsar;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal class PulsarBindings_Should
    {
        [Test]
        public void PulsarChannelBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  pulsar:
    namespace: staging
    persistence: persistent
    compaction: 1000
    geo-replication:
      - us-east1
      - us-west1
    retention:
      time: 7
      size: 1000
    ttl: 360
    deduplication: true
    bindingVersion: '0.1.0'";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new PulsarChannelBinding
            {
                Namespace = "staging",
                Persistence = Persistence.Persistent,
                Compaction = 1000,
                GeoReplication = new List<string>
                {
                    "us-east1",
                    "us-west1",
                },
                Retention = new RetentionDefinition()
                {
                    Time = 7,
                    Size = 1000,
                },
                TTL = 360,
                Deduplication = true,
                BindingVersion = "0.1.0",
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var binding = new AsyncApiStringReader().ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(channel);
        }

        [Test]
        public void PulsarServerBinding_WithFilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"url: https://example.com
protocol: pulsar
bindings:
  pulsar:
    tenant: contoso";

            var server = new AsyncApiServer()
            {
                Url = "https://example.com",
                Protocol = "pulsar",
            };

            server.Bindings.Add(new PulsarServerBinding
            {
                Tenant = "contoso",
            });

            // Act
            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var binding = new AsyncApiStringReader().ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(actual, expected);
            binding.Should().BeEquivalentTo(server);
        }
    }
}
