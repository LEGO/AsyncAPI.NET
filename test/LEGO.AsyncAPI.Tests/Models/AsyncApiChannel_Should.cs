using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    internal class AsyncApiChannel_Should
    {
        [Test]
        public void AsyncApiChannel_WithBindings_Serializes()
        {
            var expected =
@"bindings:
  kafka:
    topic: topic
    partitions: 5
    replicas: 2";

            var channel = new AsyncApiChannel
            {
                Bindings = new AsyncApiBindings<IChannelBinding>
                {
                    {
                        new KafkaChannelBinding
                        {
                            Topic = "topic",
                            Partitions = 5,
                            Replicas = 2,
                        }
                    },

                },
            };

            var actual = channel.SerializeAsYaml();

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
        }
    }
}
