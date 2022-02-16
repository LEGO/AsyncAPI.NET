using System.Collections.Generic;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models.Any;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests;

public static class MockData
{
    public static Dictionary<string, IAny> Extensions()
    {
        return new Dictionary<string, IAny>
        {
            {"x-ext-null", new Null()},
            {"x-ext-integer", (Long)13},
            {"x-ext-number", (Double)13.13},
            {"x-ext-string", (String)"bar"},
            {"x-ext-boolean", (Boolean)true},
            {"x-ext-array", new Array() { (String)"foo", new Object{ ["foo"] = (String)"bar" } }},
            {"x-ext-object", new Object{ ["foo"] = (String)"bar" } }
        };
    }

    public static Object Payload()
    {
        var payload = new Object();
        payload.Add("foo", (String)"bar");
        payload.Add("baz", (Long)13);
        payload.Add("bazz", (Double)13.13);
        payload.Add("grault", new Object
        {
            { "garply", (String)"waldo" }
        });
        payload.Add("qux", new Array {(String)"foo" });
        payload.Add("quux", (Boolean)true);
        payload.Add("quuz", new Null());
        return payload;
    }
}