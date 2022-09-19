using System;
using System.Globalization;
using System.IO;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Writers;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiOperation_Should
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
            var expected = @"message:
  oneOf:
    - name: First Message
    - name: Second Message";

            var asyncApiOperation = new AsyncApiOperation();
            asyncApiOperation.Message.Add(new AsyncApiMessage { Name = "First Message" });
            asyncApiOperation.Message.Add(new AsyncApiMessage { Name = "Second Message" });
            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputString);

            // Act
            asyncApiOperation.SerializeV2(writer);

            // Assert
            var actual = outputString.GetStringBuilder().ToString();
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void SerializeV2_WithSingleMessage_Serializes()
        {
            // Arrange
            var expected = @"message:
  name: First Message";

            var asyncApiOperation = new AsyncApiOperation();
            asyncApiOperation.Message.Add(new AsyncApiMessage { Name = "First Message" });
            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputString);

            // Act
            asyncApiOperation.SerializeV2(writer);

            // Assert
            var actual = outputString.GetStringBuilder().ToString();
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            Assert.AreEqual(actual, expected);
        }
    }
}
