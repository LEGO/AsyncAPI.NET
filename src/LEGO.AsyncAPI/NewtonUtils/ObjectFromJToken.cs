namespace LEGO.AsyncAPI.NewtonUtils
{
    using Newtonsoft.Json.Linq;

    public static class ObjectFromJToken
    {
        public static T Map<T>(JToken token)
        {
            return token.ToObject<T>(JsonSerializerUtils.GetSerializer());
        }
    }
}