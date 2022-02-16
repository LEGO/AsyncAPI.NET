using System;
using LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiExternalDocsObject;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiSecuritySchemaObject
{
    public class ShouldProduceSecuritySchema: ShouldConsumeProduceBase<SecurityScheme>
    {
        public ShouldProduceSecuritySchema(): base(typeof(ShouldProduceSecuritySchema))
        {
        }

        [Fact]
        public async void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new SecurityScheme(
                SecuritySchemeType.Http,
                "baz",
                "quz",
                "quuz",
                new OAuthFlows(), 
                new Uri("https://lego.com")
                )));
        }
        
        [Fact]
        public async void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new SecurityScheme(
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