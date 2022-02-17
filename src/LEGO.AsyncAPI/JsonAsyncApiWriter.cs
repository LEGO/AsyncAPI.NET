namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;

    public class JsonAsyncApiWriter<T> : IAsyncApiWriter<T>
    {
        public string Write(T asyncApiDocument)
        {
            return JsonConvert.SerializeObject(asyncApiDocument, JsonSerializerUtils.GetSettings());
        }
    }
}