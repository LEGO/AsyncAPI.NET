namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.Any;
    using LEGO.AsyncAPI.Models;
    using Newtonsoft.Json;

    public class AsyncApiAsyncApiReaderNewtonJson<T>: IAsyncApiReader<T>
    {
        public T Consume(Stream stream)
        {
            return JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd(), new PayloadConverter());
        }
    }
}