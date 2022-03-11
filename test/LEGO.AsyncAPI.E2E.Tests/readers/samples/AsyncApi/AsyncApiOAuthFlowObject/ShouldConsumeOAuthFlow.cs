namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOAuthFlowObject
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Xunit;

    public class ShouldConsumeOAuthFlow: ShouldConsumeProduceBase<OAuthFlow>
    {
        public ShouldConsumeOAuthFlow(): base(typeof(ShouldConsumeOAuthFlow))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(AsyncApiAsyncApiReader.Read(GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal(new Uri("https://lego.com/auth"), output.AuthorizationUrl);
            Assert.Equal(new Uri("https://lego.com/token"),output.TokenUrl);
            Assert.Equal(new Uri("https://lego.com/refresh"),output.RefreshUrl);
            Assert.IsType<Dictionary<string, string>>(output.Scopes);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}