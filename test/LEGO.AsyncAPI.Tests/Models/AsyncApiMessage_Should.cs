using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.Http;
using LEGO.AsyncAPI.Models.Bindings.Kafka;
using LEGO.AsyncAPI.Models.Interfaces;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{

    internal class AsyncApiMessage_Should
    {
        [Test]
        public void AsyncApiMessage_WithBindings_Serializes()
        {
            var expected =
@"bindings:
  http:
    headers:
      description: this mah binding
  kafka:
    key:
      description: this mah other binding
    SchemaIdLocation: test
    schemaIdPayloadEncoding: test
    schemaLookupStrategy: header";
            var message = new AsyncApiMessage
            {
                Bindings = new AsyncApiBindings<IMessageBinding>
                {
                    {
                        new HttpMessageBinding
                        {
                            Headers = new AsyncApiSchema
                            {
                                Description = "this mah binding",
                            }
                        }
                    },
                    {
                        new KafkaMessageBinding
                        {
                            Key = new AsyncApiSchema
                            {
                                Description = "this mah other binding",
                            },
                            SchemaIdLocation = "test",
                            SchemaIdPayloadEncoding = "test",
                            SchemaLookupStrategy = "header",
                        }
                    },

                },
            };

            var actual = message.SerializeAsYaml();

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
        }
    }
}
