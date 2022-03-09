namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiSecuritySchemaObject
{
    using System;
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceSecuritySchema: ShouldConsumeProduceBase<SecurityScheme>
    {
        public ShouldProduceSecuritySchema(): base(typeof(ShouldProduceSecuritySchema))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new SecurityScheme(
                SecuritySchemeType.Http,
                "baz",
                "quz",
                "quuz",
                new OAuthFlows(), 
                new Uri("https://lego.com")
                )));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(new SecurityScheme(
                SecuritySchemeType.Http,
                "baz",
                "quz",
                "quuz",
                new OAuthFlows(), 
                new Uri("https://lego.com")
                )
            {
                Description = "bar",
                BearerFormat = "qoz",
                Extensions = MockData.Extensions(),
            }));
        }
    }
}