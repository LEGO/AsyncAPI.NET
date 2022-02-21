namespace LEGO.AsyncAPI
{
    public interface IAsyncApiWriter<T>
    {
        string Write(T asyncApiDocument);
    }
}