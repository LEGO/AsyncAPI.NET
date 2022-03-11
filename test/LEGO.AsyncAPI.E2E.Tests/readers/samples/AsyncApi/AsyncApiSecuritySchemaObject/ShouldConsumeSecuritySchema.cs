namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiSecuritySchemaObject
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using Xunit;

    public class ShouldConsumeSecuritySchema : ShouldConsumeProduceBase<SecurityScheme>
    {
        public ShouldConsumeSecuritySchema()
            : base(typeof(ShouldConsumeSecuritySchema))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal(SecuritySchemeType.Http, output.Type);
            Assert.Equal("bar", output.Description);
            Assert.Equal("baz", output.Name);
            Assert.Equal("quz", output.In);
            Assert.Equal("quuz", output.Scheme);
            Assert.Equal("qoz", output.BearerFormat);
            Assert.IsType<OAuthFlows>(output.Flows);
            Assert.Equal(new Uri("https://lego.com"), output.OpenIdConnectUrl);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}