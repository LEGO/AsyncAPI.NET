namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi
{
    using Models;
    using Xunit;

    public class ShouldProduceConsumeAsyncApiDocument : ShouldConsumeProduceBase<AsyncApiDocument>
    {
        public ShouldProduceConsumeAsyncApiDocument() : base(typeof(ShouldProduceAsyncApiDocument))
        {
        }

        [Fact]
        public void ShouldProduceConsumeCompleteSpec()
        {
            var doc = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = _asyncApiWriter.Write(doc);

            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), serializedDoc);
        }

        [Fact]
        public void ShouldProduceConsumeTwiceCompleteSpec()
        {
            var doc = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = _asyncApiWriter.Write(doc);

            var doc2 = _asyncApiAsyncApiReader.Read(GenerateStreamFromString(serializedDoc));

            var serializedDoc2 = _asyncApiWriter.Write(doc2);

            Assert.Equal(serializedDoc, serializedDoc2);
        }
    }
}