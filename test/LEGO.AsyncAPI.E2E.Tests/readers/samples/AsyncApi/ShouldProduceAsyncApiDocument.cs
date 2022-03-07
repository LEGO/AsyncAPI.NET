using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi
{
    using Models.Any;

    public class ShouldProduceAsyncApiDocument : ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldProduceAsyncApiDocument() : base(typeof(ShouldProduceAsyncApiDocument))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"),
                asyncApiWriter.Write(new AsyncApiDocument
                    {Asyncapi = "2.3.0", Info = new Info("foo", "bar")}));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), asyncApiWriter.Write(new AsyncApiDocument
            {
                Asyncapi = "2.3.0",
                Id = "urn:com:smartylighting:streetlights:server",
                Info = MockData.Info(MockData.Contact(false), MockData.License(false), false),
                Servers = new Dictionary<string, Server>
                {
                    {
                        "production",
                        MockData.Server(
                            new Dictionary<string, ServerVariable> {{"username", MockData.ServerVariable(false)}},
                            new List<SecurityRequirement> { new() {Extensions = new Dictionary<string, IAny> {{ "petstore_auth", new Array {(String)"write:pets", (String)"read:pets"}}}}},
                            false)
                    }
                },
                DefaultContentType = "application/json",
                Channels = new Dictionary<string, Channel>(){{"subscribe", MockData.Channel(false)}},
                Components = MockData.Components(false),
                Tags = new [] { MockData.Tag(false) },
                ExternalDocs = MockData.ExternalDocs(false),
                Extensions = MockData.Extensions()
            }));
        }
    }
}