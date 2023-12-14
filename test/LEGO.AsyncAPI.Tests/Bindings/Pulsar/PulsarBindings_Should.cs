// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Tests.Bindings.Pulsar
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Pulsar;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.Pulsar;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

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
    bindingVersion: 0.1.0";

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

            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Pulsar;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }

        [Test]
        public void PulsarChannelBindingNamespaceDefaultToNull()
        {
            // Arrange
            var actual =
                @"bindings:
  pulsar:
    persistence: persistent";

            // Act
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Pulsar;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(null, ((PulsarChannelBinding)binding.Bindings["pulsar"]).Namespace);
        }

        [Test]
        public void PulsarChannelBindingPropertiesExceptNamespaceDefaultToNull()
        {
            // Arrange
            var actual =
                @"bindings:
  pulsar:
    namespace: staging";

            // Act
            // Assert
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Pulsar;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);
            var pulsarBinding = ((PulsarChannelBinding)binding.Bindings["pulsar"]);

            Assert.AreEqual("staging", pulsarBinding.Namespace);
            Assert.AreEqual(null, pulsarBinding.Persistence);
            Assert.AreEqual(null, pulsarBinding.Compaction);
            Assert.AreEqual(null, pulsarBinding.GeoReplication);
            Assert.AreEqual(null, pulsarBinding.Retention);
            Assert.AreEqual(null, pulsarBinding.TTL);
            Assert.AreEqual(null, pulsarBinding.Deduplication);
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
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Pulsar;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(server);
        }

        [Test]
        public void ServerBindingVersionDefaultsToNull()
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
                BindingVersion = null,
            });

            // Act
            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Pulsar;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(null, ((PulsarServerBinding)binding.Bindings["pulsar"]).BindingVersion);
            binding.Should().BeEquivalentTo(server);
        }

        [Test]
        public void ServerTenantDefaultsToNull()
        {
            // Arrange
            var expected =
                @"url: https://example.com
protocol: pulsar
bindings:
  pulsar:
    bindingVersion: latest";

            var server = new AsyncApiServer()
            {
                Url = "https://example.com",
                Protocol = "pulsar",
            };

            server.Bindings.Add(new PulsarServerBinding
            {
                Tenant = null,
                BindingVersion = "latest",
            });

            // Act
            var actual = server.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = BindingsCollection.Pulsar;
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiServer>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(null, ((PulsarServerBinding)binding.Bindings["pulsar"]).Tenant);
            binding.Should().BeEquivalentTo(server);
        }
    }
}
