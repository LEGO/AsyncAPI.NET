namespace LEGO.AsyncAPI.Tests.Bindings
{
    using System;
    using System.Collections.Generic;
    using AsyncAPI.Models.Any;
    using AsyncAPI.Models.Interfaces;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class MyBinding : ChannelBinding<MyBinding>
    {
        public string Custom { get; set; }

        public override string Type => "my";

        public override string BindingVersion { get; set; }

        protected override FixedFieldMap<MyBinding> FixedFieldMap => new FixedFieldMap<MyBinding>()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "custom", (a, n) => { a.Custom = n.GetScalarValue(); } },
        };

        public override void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteRequiredProperty("custom", this.Custom);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public class CustomBinding_Should
    {
        [Test]
        public void CustomBinding_SerializesDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  my:
    custom: someValue
    bindingVersion: '0.1.0'
    x-myextension: someValue";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new MyBinding
            {
                Custom = "someValue",
                BindingVersion = "0.1.0",
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    { "x-myextension", new AsyncApiString("someValue") },
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            var settings = new AsyncApiReaderSettings();
            settings.Bindings.Add(new MyBinding());
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            Assert.AreEqual(expected, actual);
            binding.Should().BeEquivalentTo(channel);
        }
    }
}
