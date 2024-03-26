// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Bindings
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class NestedConfiguration : IAsyncApiExtensible
    {
        public string Name { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public static FixedFieldMap<NestedConfiguration> FixedFieldMap = new ()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
        };

        public void SerializeProperties(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("name", this.Name);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public class MyBinding : ChannelBinding<MyBinding>
    {
        public string Custom { get; set; }

        public override string BindingKey => "my";

        public NestedConfiguration NestedConfiguration { get; set; }

        public AsyncApiAny Any { get; set; }

        protected override FixedFieldMap<MyBinding> FixedFieldMap => new FixedFieldMap<MyBinding>()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "custom", (a, n) => { a.Custom = n.GetScalarValue(); } },
            { "any", (a, n) => { a.Any = n.CreateAny(); } },
            { "nestedConfiguration", (a, n) => { a.NestedConfiguration = n.ParseMapWithExtensions(NestedConfiguration.FixedFieldMap); } },
        };

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteRequiredProperty("custom", this.Custom);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);
            writer.WriteRequiredObject("any", this.Any, (w, p) => w.WriteAny(p));
            writer.WriteOptionalObject("nestedConfiguration", this.NestedConfiguration, (w, r) => r.SerializeProperties(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public class CustomBinding_Should : TestBase
    {
        [Test]
        public void CustomBinding_SerializesDeserializes()
        {
            // Arrange
            var expected =
@"bindings:
  my:
    custom: someValue
    bindingVersion: 0.1.0
    any:
      anyKeyName: anyValue
    nestedConfiguration:
      name: nested
      x-myNestedExtension: nestedValue
    x-myextension: someValue";

            var channel = new AsyncApiChannel();
            channel.Bindings.Add(new MyBinding
            {
                Custom = "someValue",
                Any = new AsyncApiObject()
                {
                    { "anyKeyName", new AsyncApiAny("anyValue") },
                },
                BindingVersion = "0.1.0",
                NestedConfiguration = new NestedConfiguration()
                {
                    Name = "nested",
                    Extensions = new Dictionary<string, IAsyncApiExtension>()
                    {
                        { "x-myNestedExtension", new AsyncApiAny("nestedValue") },
                    },
                },
                Extensions = new Dictionary<string, IAsyncApiExtension>()
                {
                    { "x-myextension", new AsyncApiAny("someValue") },
                },
            });

            // Act
            var actual = channel.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            var settings = new AsyncApiReaderSettings();
            settings.Bindings = new[] { new MyBinding() };
            var binding = new AsyncApiStringReader(settings).ReadFragment<AsyncApiChannel>(actual, AsyncApiVersion.AsyncApi2_0, out _);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
            binding.Should().BeEquivalentTo(channel);
        }
    }
}
