﻿using FluentAssertions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.ParseNodes;
using NUnit.Framework;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEGO.AsyncAPI.Tests
{
    public class AsyncApiLicenseTests
    {
        [Test]
        public void Serialize_WithAllProperties_Serializes()
        {
            var expected = @"{
  ""name"": ""test"",
  ""url"": ""https://example.com/license"",
  ""x-extension"": ""value""
}";
            var license = new AsyncApiLicense()
            {
                Name = "test",
                Url = new Uri("https://example.com/license"),
                Extensions = new Dictionary<string, IAsyncApiExtension>
                {
                    ["x-extension"] = new AsyncApiString("value"),
                },
            };

            var actual = license.SerializeAsJson();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            Assert.AreEqual(expected, actual);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [Test]
        public void LoadLicense_WithJson_Deserializes()
        {
            // Arrange
           var input = @"{
  ""name"": ""test"",
  ""url"": ""https://example.com/license"",
  ""x-extension"": ""value""
}";

            using (var stream = GenerateStreamFromString(input))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var actual = AsyncApiDeserializer.LoadLicense(node);

                // Assert
                var expected = new AsyncApiLicense()
                {
                    Name = "test",
                    Url = new Uri("https://example.com/license"),
                    Extensions = new Dictionary<string, IAsyncApiExtension>
                    {
                        ["x-extension"] = new AsyncApiString("value"),
                    },
                };
                actual.Should().BeEquivalentTo(
                    expected);
            }
        }
    }
}