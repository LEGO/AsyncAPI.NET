namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceAsyncApiDocument : ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldProduceAsyncApiDocument()
            : base(typeof(ShouldProduceAsyncApiDocument))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(
                this.GetString("Minimal.json"),
                this.AsyncApiWriter.Write(new AsyncApiDocument
                { Asyncapi = "2.3.0", Info = new AsyncApiInfo("foo", "bar") }));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new AsyncApiDocument
            {
                Asyncapi = "2.3.0",
                Id = "urn:com:smartylighting:streetlights:server",
                Info = MockData.Info(MockData.Contact(false), MockData.License(false), false),
                Servers = new Dictionary<string, AsyncApiServer>
                {
                    {
                        "production",
                        MockData.Server(
                            new Dictionary<string, AsyncApiServerVariable> { { "username", MockData.ServerVariable(false) } },
                            new List<Dictionary<string, string[]>> { new () { { "petstore_auth", new[] { "write:pets", "read:pets" } } } },
                            false)
                    },
                },
                DefaultContentType = "application/json",
                Channels = new Dictionary<string, AsyncApiChannel> { { "subscribe", MockData.Channel(false) } },
                Components = MockData.Components(false),
                Tags = new[] { MockData.Tag(false) },
                ExternalDocs = MockData.ExternalDocs(false),
                Extensions = MockData.Extensions(),
            }));
        }
    }
}