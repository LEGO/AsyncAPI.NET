using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi
{
    public class ShouldProduceAsyncApiDocument : ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldProduceAsyncApiDocument() : base(typeof(ShouldProduceAsyncApiDocument))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"),
                _asyncApiWriter.Produce(new AsyncApiDocument
                    {AsyncApi = "2.3.0", Info = new Info {Title = "foo", Version = "bar"}}));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetString("Complete.json"), _asyncApiWriter.Produce(new AsyncApiDocument
            {
                AsyncApi = "2.3.0",
                Id = "urn:com:smartylighting:streetlights:server",
                Info = new Info
                {
                    Title = "foo", Version = "bar"
                }
            }));
        }
    }
}