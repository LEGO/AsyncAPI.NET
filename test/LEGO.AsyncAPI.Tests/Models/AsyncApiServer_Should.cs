using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.Kafka;
using LEGO.AsyncAPI.Models.Interfaces;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    internal class AsyncApiServer_Should
    {
        [Test]
        public void AsyncApiServer_WithBindings_Serializes()
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

            var actual = server.SerializeAsYaml();

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
        }
    }
}
