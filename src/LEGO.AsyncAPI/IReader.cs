namespace LEGO.AsyncAPI
{
    public interface IReader<T>
    {
        public T Consume(Stream jsonDocument);
    }
}