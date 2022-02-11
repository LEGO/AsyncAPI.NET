using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

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

    public static JToken Payload()
    {
        var payload = new JObject();
        payload.Add("foo", "bar");
        payload.Add("baz", 13);
        payload.Add("bazz", 13.13);
        payload.Add("grault", new JObject
        {
            { "garply", JValue.CreateString("waldo") }
        });
        payload.Add("qux", new JArray { JValue.CreateString("foo") });
        payload.Add("quux", true);
        payload.Add("quuz", null);
        return payload;
    }
}