namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Writers;

    public interface IAsyncApiExtension
    {
        void Write(IAsyncApiWriter writer, AsyncApiVersion specVersion);
    }
}