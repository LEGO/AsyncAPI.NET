namespace LEGO.AsyncAPI.Tests.Bindings
{
    using System;
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
            writer.WriteEndObject();
        }
    }

    internal class CustomBinding_Should
    {
        [Test]
        public void CustomBinding_SerializesDeserializes()
        {
            // Arrange
            var actual =
                @"bindings:
  my:
    bindingVersion: 0.1.0
    custom: someValue";

            // Act
            // Assert
            var settings = new AsyncApiReaderSettings();
            settings.BindingParsers.Add(new MyBinding());
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);
            var myBinding = ((MyBinding)binding.Bindings["my"]);

            Assert.AreEqual("someValue", myBinding.Custom);
            Assert.AreEqual("0.1.0", myBinding.BindingVersion);
        }
    }
}
