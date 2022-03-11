namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOAuthFlowsObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using Xunit;

    public class ShouldConsumeOAuthFlows : ShouldConsumeProduceBase<OAuthFlows>
    {
        public ShouldConsumeOAuthFlows()
            : base(typeof(ShouldConsumeOAuthFlows))
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

            Assert.IsType<OAuthFlow>(output.Implicit);
            Assert.IsType<OAuthFlow>(output.Password);
            Assert.IsType<OAuthFlow>(output.ClientCredentials);
            Assert.IsType<OAuthFlow>(output.AuthorizationCode);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}