using LEGO.AsyncAPI.NewtonUtils;
using Newtonsoft.Json;

namespace LEGO.AsyncAPI
{
    public class JsonAsyncApiWriter<T> : IAsyncApiWriter<T>
    {
        public string Write(T asyncApiDocument)
        {
            return JsonConvert.SerializeObject(asyncApiDocument, JsonSerializerUtils.GetSettings());
        }
    }
}