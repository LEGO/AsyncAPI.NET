namespace LEGO.AsyncAPI.Readers;

public interface IAsyncApiExternalReferenceReader
{
    public string GetExternalResource(string reference);
}