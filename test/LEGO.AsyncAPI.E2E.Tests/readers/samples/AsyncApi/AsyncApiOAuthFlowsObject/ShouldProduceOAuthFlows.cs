namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOAuthFlowsObject
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceOAuthFlows : ShouldConsumeProduceBase<OAuthFlows>
    {
        public ShouldProduceOAuthFlows()
            : base(typeof(ShouldProduceOAuthFlows))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new OAuthFlows()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new OAuthFlows
            {
                Implicit = new OAuthFlow(),
                Password = new OAuthFlow(),
                ClientCredentials = new OAuthFlow(),
                AuthorizationCode = new OAuthFlow(),
                Extensions = MockData.Extensions(),
            }));
        }
    }
}