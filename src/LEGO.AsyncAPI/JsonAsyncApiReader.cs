namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonAsyncApiReader<T> : IAsyncApiReader<T>
    {
        public T Read(Stream stream)
        {
            var jsonString = new StreamReader(stream).ReadToEnd();
            var root = JsonConvert.DeserializeObject<JToken>(jsonString);
            root.ResolveReferences();

            return JsonConvert.DeserializeObject<T>(root.ToString(), JsonSerializerUtils.GetSettings());
        }
    }
}