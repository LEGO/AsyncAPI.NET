namespace LEGO.AsyncAPI.Readers;

using Models;
using Stubs;

/// <summary>
/// Internal class, converts contents on JSON string stream to AsyncApiDocument instance 
/// </summary>
internal class AsyncApiJsonReader : Interface.IAsyncApiReader<Stream>
{
    /// <summary>
    /// Converts AsyncApi JSON definition into an instance of AsyncApiDocument.
    /// </summary>
    /// <param name="input">Stream containing AsyncApi JSON definition to parse.</param>
    /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
    /// <returns>Instance of newly created AsyncApiDocument.</returns>
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