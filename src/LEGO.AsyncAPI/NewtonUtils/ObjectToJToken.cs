namespace LEGO.AsyncAPI.NewtonUtils
{
    using Newtonsoft.Json.Linq;

    public static class ObjectToJToken
    {
        public static JToken Map(object o)
        {
            return o == null ? JToken.Parse("null") : JToken.FromObject(o, JsonSerializerUtils.GetSerializer());
        }
    }
}