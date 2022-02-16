namespace LEGO.AsyncAPI.Readers;

using Models;
using Stubs;

internal class AsyncApiJsonReader : Interface.IAsyncApiReader<Stream>
{
    public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
    {
        diagnostic = new AsyncApiDiagnostic();

        AsyncApiDocument document;

        var internalAsyncApiReader = new InternalAsyncApiStreamReaderMock();
        
        try
        {
            document = internalAsyncApiReader.Read(input);
        }
        catch (Exception e)
        {
            diagnostic = AsyncApiDiagnostic.OnError(e);
            return new AsyncApiDocument();
        }

        return document;
    }
}