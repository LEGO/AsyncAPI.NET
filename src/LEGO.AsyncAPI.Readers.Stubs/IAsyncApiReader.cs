namespace LEGO.AsyncAPI.Readers.Stubs
{
    using Models;

    public interface IAsyncApiReader
    {
        public AsyncApiDocument Read(Stream stream);
    }
}
