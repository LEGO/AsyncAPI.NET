namespace LEGO.AsyncAPI.Readers.Stubs;

using Models;

public class InternalAsyncApiStreamReaderMock : IAsyncApiReader
{
    public AsyncApiDocument Read(Stream stream)
    {
        throw new NotImplementedException();
    }
}