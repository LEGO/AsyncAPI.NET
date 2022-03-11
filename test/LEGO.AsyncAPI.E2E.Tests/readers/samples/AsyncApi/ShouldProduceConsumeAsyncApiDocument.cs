namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi
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
            var doc = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = AsyncApiWriter.Write(doc);

            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), serializedDoc);
        }

        [Fact]
        public void ShouldProduceConsumeTwiceCompleteSpec()
        {
            var doc = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            var serializedDoc = AsyncApiWriter.Write(doc);

            var doc2 = AsyncApiAsyncApiReader.Read(GenerateStreamFromString(serializedDoc));

            var serializedDoc2 = AsyncApiWriter.Write(doc2);

            Assert.Equal(serializedDoc, serializedDoc2);
        }
    }
}