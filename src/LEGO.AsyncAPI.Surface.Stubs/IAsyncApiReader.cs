namespace LEGO.AsyncAPI.Surface.Stubs
{
    public interface IAsyncApiReader<T>
    {
        public T Consume(Stream jsonDocument);
    }
}