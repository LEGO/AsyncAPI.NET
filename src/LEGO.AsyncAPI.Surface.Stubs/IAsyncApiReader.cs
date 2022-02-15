namespace LEGO.AsyncAPI.Surface.Stubs
{
    using System.Text.Json;
    using Newtonsoft.Json.Linq;

    public interface IAsyncApiReader<T>
    {
        public T Consume(Stream jsonDocument);
        public T Consume(JObject jObject);
    }
}