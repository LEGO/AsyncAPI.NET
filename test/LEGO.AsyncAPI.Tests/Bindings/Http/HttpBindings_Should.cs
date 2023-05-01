namespace LEGO.AsyncAPI.Tests.Bindings.Http
{
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Http;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    internal class HttpBindings_Should
    {
        [Test]
        public void HttpMessageBinding_FilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  http:
    headers:
      description: this mah binding";

            var message = new AsyncApiMessage();

            message.Bindings.Add(new HttpMessageBinding
            {
                Headers = new AsyncApiSchema
                {
                    Description = "this mah binding",
                },
            });

            // Act
            var actual = message.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(BindingsCollection.Http);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiMessage>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(message);
        }

        [Test]
        public void HttpOperationBinding_FilledObject_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  http:
    type: request
    method: POST
    query:
      description: this mah query";

            var operation = new AsyncApiOperation();

            operation.Bindings.Add(new HttpOperationBinding
            {
                Type = HttpOperationBinding.HttpOperationType.Request,
                Method = "POST",
                Query = new AsyncApiSchema
                {
                    Description = "this mah query",
                },
            });

            // Act
            var actual = operation.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(BindingsCollection.Http);
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiOperation>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(operation);
        }
    }
}
