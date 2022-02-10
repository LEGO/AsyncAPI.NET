namespace LEGO.AsyncAPI
{
    public interface IAsyncApiReader<T>
    {
        public T Consume(Stream jsonDocument);
    }
}