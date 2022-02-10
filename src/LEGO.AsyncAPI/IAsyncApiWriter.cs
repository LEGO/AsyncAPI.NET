namespace LEGO.AsyncAPI;

public interface IAsyncApiWriter<T>
{
    string Produce(T asyncApiDocument);
}