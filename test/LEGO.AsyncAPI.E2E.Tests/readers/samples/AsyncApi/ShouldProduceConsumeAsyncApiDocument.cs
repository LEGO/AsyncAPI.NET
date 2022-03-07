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
            var doc = asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = asyncApiWriter.Write(doc);

            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), serializedDoc);
        }

        [Fact]
        public void ShouldProduceConsumeTwiceCompleteSpec()
        {
            var doc = asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = asyncApiWriter.Write(doc);

            var doc2 = asyncApiAsyncApiReader.Read(GenerateStreamFromString(serializedDoc));

            var serializedDoc2 = asyncApiWriter.Write(doc2);

            Assert.Equal(serializedDoc, serializedDoc2);
        }
    }
}