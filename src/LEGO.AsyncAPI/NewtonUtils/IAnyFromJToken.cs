namespace LEGO.AsyncAPI.NewtonUtils
{
    using LEGO.AsyncAPI.Models.Any;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Array = LEGO.AsyncAPI.Models.Any.Array;
    using Boolean = LEGO.AsyncAPI.Models.Any.Boolean;
    using Double = LEGO.AsyncAPI.Models.Any.Double;
    using Object = LEGO.AsyncAPI.Models.Any.Object;
    using String = LEGO.AsyncAPI.Models.Any.String;

    public static class IAnyFromJToken
    {
        public static IAny Map(JToken token)
        {
            return token.Type switch
            {
                // Unfortunately we cannot us the Object mapper for this. Newton will always from an exception using the value is null
                // see: ConvertUtils.ConvertResult.TryConvertInternal for details
                JTokenType.Null => default(Null),
                JTokenType.String => ObjectFromJToken.Map<String>(token),
                JTokenType.Boolean => ObjectFromJToken.Map<Boolean>(token),
                JTokenType.Integer => ObjectFromJToken.Map<Long>(token),
                JTokenType.Float => ObjectFromJToken.Map<Double>(token),
                JTokenType.Object => Map(token.Value<JObject>()),
                JTokenType.Array => Map(token.Value<JArray>()),
                _ => throw new JsonException("Value of type " + token.GetType() + " not handled for arrays")
            };
        }

        private static Object Map(JObject value)
        {
            var output = new Object();
            foreach (var jProperty in value.Properties())
            {
                output.Add(jProperty.Name, Map(jProperty.Value));
            }

            return output;
        }

        private static Array Map(JArray value)
        {
            var output = new Array();
            output.AddRange(value.Select(Map));
            return output;
        }
    }
}