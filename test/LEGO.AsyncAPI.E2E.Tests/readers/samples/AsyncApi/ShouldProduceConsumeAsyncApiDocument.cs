namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi
{
    using LEGO.AsyncAPI.Models;
    using Xunit;

    public class ShouldProduceConsumeAsyncApiDocument : ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldProduceConsumeAsyncApiDocument()
            : base(typeof(ShouldProduceAsyncApiDocument))
        {
        }

        [Fact]
        public void ShouldProduceConsumeCompleteSpec()
        {
            var doc = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = this.AsyncApiWriter.Write(doc);

            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), serializedDoc);
        }

        [Fact]
        public void ShouldProduceConsumeTwiceCompleteSpec()
        {
            var doc = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = this.AsyncApiWriter.Write(doc);

            var doc2 = this.AsyncApiAsyncApiReader.Read(GenerateStreamFromString(serializedDoc));

            var serializedDoc2 = this.AsyncApiWriter.Write(doc2);

            Assert.Equal(serializedDoc, serializedDoc2);
        }
    }
}