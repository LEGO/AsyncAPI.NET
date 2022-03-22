// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.NewtonUtils
{
    using System;
    using LEGO.AsyncAPI.Models.Any;
    using Newtonsoft.Json.Linq;

    internal static class AnyToJToken
    {
        public static JToken Map(IAny o)
        {
            if (o == null)
            {
                return ObjectToJToken.Map(null);
            }

            return o.AnyType switch
            {
                AnyType.Primitive => (IPrimitive)o switch
                {
                    AsyncAPIString s => ObjectToJToken.Map(s.Value),
                    AsyncAPILong l => ObjectToJToken.Map(l.Value),
                    AsyncAPIDouble d => ObjectToJToken.Map(d.Value),
                    AsyncAPIBoolean b => ObjectToJToken.Map(b.Value),
                    AsyncAPINull n => ObjectToJToken.Map(n.Value),
                    _ => throw new ArgumentOutOfRangeException()
                },
                AnyType.Array => Map(o as AsyncAPIArray),
                AnyType.Object => Map(o as AsyncAPIObject),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static JToken Map(AsyncAPIArray obj)
        {
            JArray tokenArray = new();

            foreach (var item in obj)
            {
                tokenArray.Add(Map(item));
            }

            return tokenArray;
        }

        private static JToken Map(AsyncAPIObject obj)
        {
            JObject tokenObject = new();

            foreach (var key in obj.Keys)
            {
                tokenObject.Add(key, Map(obj[key]));
            }

            return tokenObject;
        }
    }
}