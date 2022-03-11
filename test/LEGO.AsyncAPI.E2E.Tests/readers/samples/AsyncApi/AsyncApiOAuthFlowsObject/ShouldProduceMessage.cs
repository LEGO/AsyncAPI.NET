namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOAuthFlowsObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceOAuthFlows: ShouldConsumeProduceBase<OAuthFlows>
    {
        public ShouldProduceOAuthFlows(): base(typeof(ShouldProduceOAuthFlows))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), AsyncApiWriter.Write(new OAuthFlows()));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), AsyncApiWriter.Write(new OAuthFlows
            {
                Implicit = new OAuthFlow(),
                Password = new OAuthFlow(),
                ClientCredentials = new OAuthFlow(),
                AuthorizationCode = new OAuthFlow(),
                Extensions = MockData.Extensions()
            }));
        }
    }
}