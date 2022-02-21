using System;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOAuthFlowObject
{
    public class ShouldProduceOAuthFlow: ShouldConsumeProduceBase<OAuthFlow>
    {
        public ShouldProduceOAuthFlow(): base(typeof(ShouldProduceOAuthFlow))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new OAuthFlow()));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new OAuthFlow
            {
                AuthorizationUrl = new Uri("https://lego.com/auth"),
                TokenUrl = new Uri("https://lego.com/token"),
                RefreshUrl = new Uri("https://lego.com/refresh"),
                Scopes = ImmutableDictionary<string, string>.Empty,
                Extensions = MockData.Extensions()
            }));
        }
    }
}