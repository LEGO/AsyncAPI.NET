// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json.Nodes;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using NUnit.Framework;

    public class AsyncApiLicenseTests : TestBase
    {
        [Test]
        public void Serialize_WithAllProperties_Serializes()
        {
            var expected = """
                {
                  "name": "test",
                  "url": "https://example.com/license",
                  "x-extension": "value"
                }
                """;
            var license = new AsyncApiLicense()
            {
                Name = "test",
                Url = new Uri("https://example.com/license"),
                Extensions = new Dictionary<string, IAsyncApiExtension>
                {
                    ["x-extension"] = new AsyncApiAny("value"),
                },
            };

            var actual = license.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                .BePlatformAgnosticEquivalentTo(expected);
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
            var input = """
                {
                  "name": "test",
                  "url": "https://example.com/license",
                  "x-extension": "value"
                }
                """;

            using (var stream = GenerateStreamFromString(input))
            {
                var diagnostic = new AsyncApiDiagnostic();
                var settings = new AsyncApiReaderSettings();
                var context = new ParsingContext(diagnostic, settings);

                var node = new MapNode(context, JsonNode.Parse(stream));

                // Act
                var actual = AsyncApiV2Deserializer.LoadLicense(node);

                // Assert
                var expected = new AsyncApiLicense()
                {
                    Name = "test",
                    Url = new Uri("https://example.com/license"),
                    Extensions = new Dictionary<string, IAsyncApiExtension>
                    {
                        ["x-extension"] = new AsyncApiAny("value"),
                    },
                };
                actual.Should().BeEquivalentTo(
                    expected);
            }
        }
    }
}
