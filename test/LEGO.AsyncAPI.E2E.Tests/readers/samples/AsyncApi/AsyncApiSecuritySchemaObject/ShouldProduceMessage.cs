namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiSecuritySchemaObject
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceSecuritySchema : ShouldConsumeProduceBase<SecurityScheme>
    {
        public ShouldProduceSecuritySchema()
            : base(typeof(ShouldProduceSecuritySchema))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new SecurityScheme(
                SecuritySchemeType.Http,
                "baz",
                "quz",
                "quuz",
                new OAuthFlows(),
                new Uri("https://lego.com"))));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new SecurityScheme(
                SecuritySchemeType.Http,
                "baz",
                "quz",
                "quuz",
                new OAuthFlows(),
                new Uri("https://lego.com"))
            {
                Description = "bar",
                BearerFormat = "qoz",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}