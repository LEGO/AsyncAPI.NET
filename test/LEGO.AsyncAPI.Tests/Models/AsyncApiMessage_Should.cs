using FluentAssertions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.Http;
using LEGO.AsyncAPI.Models.Bindings.Kafka;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    internal class AsyncApiMessage_Should
    {
        [Test]
        public void AsyncApiMessage_WithFilledObject_Serializes()
        {
            var expected = "";

            var message = new AsyncApiMessage
            {
                
            };

            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
        }
    }
}
