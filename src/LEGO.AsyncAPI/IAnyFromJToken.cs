using LEGO.AsyncAPI.Any;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Array = LEGO.AsyncAPI.Any.Array;
using Boolean = LEGO.AsyncAPI.Any.Boolean;
using Double = LEGO.AsyncAPI.Any.Double;
using Object = LEGO.AsyncAPI.Any.Object;
using String = LEGO.AsyncAPI.Any.String;

namespace LEGO.AsyncAPI
{
    public static class IAnyFromJToken
    {
        public static IAny Map(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Null => new Null(),
                JTokenType.String => new String { Value = (string)token },
                JTokenType.Boolean => new Boolean { Value = (bool)token },
                JTokenType.Integer => new Long { Value = (long)token },
                JTokenType.Float => new Double { Value = token.Value<double>() },
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