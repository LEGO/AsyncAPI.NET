using LEGO.AsyncAPI.Any;
using Newtonsoft.Json.Linq;
using Array = LEGO.AsyncAPI.Any.Array;
using Boolean = LEGO.AsyncAPI.Any.Boolean;
using Double = LEGO.AsyncAPI.Any.Double;
using Object = LEGO.AsyncAPI.Any.Object;
using String = LEGO.AsyncAPI.Any.String;

namespace LEGO.AsyncAPI;

public class IAnyToJToken
{
    public static JToken Map(IAny o)
    {
        return o.AnyType switch
        {
            AnyType.Primitive => ((Primitive) o).PrimitiveType switch
            {
                PrimitiveType.String => Map(o as String),
                PrimitiveType.Long => Map(o as Long),
                PrimitiveType.Double => Map(o as Double),
                PrimitiveType.Boolean => Map(o as Boolean),
                _ => throw new ArgumentOutOfRangeException()
            },
            AnyType.Null => Map(o as Null),
            AnyType.Array => Map(o as Array),
            AnyType.Object => Map(o as Object),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static JToken Map(String obj)
    {
        return obj.Value;
    }

    public static JToken Map(Long obj)
    {
        return obj.Value;
    }

    public static JToken Map(Double obj)
    {
        return obj.Value;
    }

    public static JToken Map(Boolean obj)
    {
        return obj.Value;
    }

    public static JToken Map(Null ignored)
    {
        return JToken.Parse("null");
    }

    public static JToken Map(Array obj)
    {
        JArray tokenArray = new JArray();

        foreach (var item in obj)
        {
            tokenArray.Add(Map(item));
        }

        return tokenArray;
    }

    public static JToken Map(Object obj)
    {
        JObject tokenObject = new JObject();

        foreach (var key in obj.Keys)
        {
            tokenObject.Add(key, Map(obj[key]));
        }

        return tokenObject;
    }
}