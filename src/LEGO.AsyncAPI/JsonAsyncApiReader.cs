namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;

    public class JsonAsyncApiReader<T> : IAsyncApiReader<T>
    {
        public T Read(Stream stream)
        {
            return JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd(), JsonSerializerUtils.GetSettings());
        }
    }
}