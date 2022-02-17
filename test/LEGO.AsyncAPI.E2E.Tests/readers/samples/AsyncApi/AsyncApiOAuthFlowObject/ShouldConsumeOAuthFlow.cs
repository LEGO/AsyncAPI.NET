using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOAuthFlowObject
{
    public class ShouldConsumeOAuthFlow: ShouldConsumeProduceBase<OAuthFlow>
    {
        public ShouldConsumeOAuthFlow(): base(typeof(ShouldConsumeOAuthFlow))
        {
        }

        [Fact]
        public async void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(_asyncApiAsyncApiReader.Read(GetStream("Minimal.json")));
        }

        [Fact]
        public async void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal(new Uri("https://lego.com/auth"), output.AuthorizationUrl);
            Assert.Equal(new Uri("https://lego.com/token"),output.TokenUrl);
            Assert.Equal(new Uri("https://lego.com/refresh"),output.RefreshUrl);
            Assert.IsType<Dictionary<string, string>>(output.Scopes);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}