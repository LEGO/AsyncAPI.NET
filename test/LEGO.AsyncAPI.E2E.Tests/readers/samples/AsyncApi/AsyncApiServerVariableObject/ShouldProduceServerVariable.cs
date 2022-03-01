namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiServerVariableObject
{
    using AsyncAPI.Tests;
    using Models;
    using Xunit;

    public class ShouldProduceServerVariable : ShouldConsumeProduceBase<ServerVariable>
    {
        public ShouldProduceServerVariable() : base(typeof(ShouldProduceServerVariable))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new ServerVariable()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.ServerVariable()));
        }
    }
}