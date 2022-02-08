using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using Newtonsoft.Json;

namespace LEGO.AsyncAPI
{
    public class AsyncApiReaderNewtonJson<T>: IReader<T>
    {
        public T Consume(Stream stream)
        {
            return JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd(), new PayloadConverter());
        }
    }
}