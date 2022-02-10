using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Tests;

public static class MockData
{
    public static Dictionary<string, JToken> Extensions()
    {
        return new Dictionary<string, JToken>
        {
            {"x-ext-null", null},
            {"x-ext-integer", 13},
            {"x-ext-number", 13.13},
            {"x-ext-string", "bar"},
            {"x-ext-boolean", true},
            {"x-ext-array", new JArray() { "foo", new JObject{ ["foo"] = "bar" } }},
            {"x-ext-object", new JObject{ ["foo"] = "bar" }}
        };
    }
}