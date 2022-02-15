namespace LEGO.AsyncAPI
{
    public interface IAsyncApiReader<T>
    {
        public T Read(Stream jsonDocument);
    }
}