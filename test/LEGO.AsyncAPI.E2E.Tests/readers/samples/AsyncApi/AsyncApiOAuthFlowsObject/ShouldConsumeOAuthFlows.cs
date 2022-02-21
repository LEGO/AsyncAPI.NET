using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOAuthFlowsObject
{
    public class ShouldConsumeOAuthFlows: ShouldConsumeProduceBase<OAuthFlows>
    {
        public ShouldConsumeOAuthFlows(): base(typeof(ShouldConsumeOAuthFlows))
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
        
            Assert.IsType<OAuthFlow>(output.Implicit);
            Assert.IsType<OAuthFlow>(output.Password);
            Assert.IsType<OAuthFlow>(output.ClientCredentials);
            Assert.IsType<OAuthFlow>(output.AuthorizationCode);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}