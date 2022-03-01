namespace LEGO.AsyncAPI.Readers.Stubs
{
    using Models;

    public class InternalAsyncApiStreamReaderMock : IAsyncApiReader
    {
        public AsyncApiDocument Read(Stream stream)
        {
            return new AsyncApiDocument() { Id = Guid.NewGuid().ToString() };
        }
    }
}
