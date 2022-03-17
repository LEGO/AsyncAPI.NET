namespace LEGO.AsyncAPI.Tests
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Resolvers;
    using Newtonsoft.Json;
    using Xunit;

    public class AsyncApiContractResolver_Should
    {
        [Fact]
        public void NotSerializeEmptyArray()
        {
            var expected = "{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\",\"non_empty_array\":[\"xyz\"],\"quux\":true,\"quuz\":null}}";

            var @object = new
            {
                foo = "bar",
                baz = 13,
                bazz = 13.13,
                grault = new
                {
                    garply = "waldo",
                    empty_array = new string[]{},
                    non_empty_array = new [] { "xyz" },
                    quux = true,
                    quuz = (object)null,
                }
            };

            var sut = new AsyncApiContractResolver();

            var actual = JsonConvert.SerializeObject(@object, new JsonSerializerSettings { ContractResolver = sut });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeMessageWithNonEmptyArrayHeaders_ThenArraysSerialized()
        {
            var expected =
                "{\"headers\":{\"allOf\":[{}],\"required\":[\"Required\"],\"enum\":[\"on\",\"off\"]}," +
                "\"payload\":{\"foo_Array\":[\"bar\"],\"foo_Array_empty\":[],\"foo_Array_with_null\":[null]},\"name\":\"foo\"}";

            var message = GetMockMessage();

            var sut = new AsyncApiContractResolver();

            var actual = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ContractResolver = sut,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeMessageWithEmptyArrayHeaders_ThenArraysNotSerialized()
        {
            var expected =
                "{\"headers\":{}," +
                "\"payload\":{\"foo_Array\":[\"bar\"],\"foo_Array_empty\":[],\"foo_Array_with_null\":[null]},\"name\":\"foo\"}";

            var message = GetMockMessage();
            message.Headers.Required = new HashSet<string>();
            message.Headers.Enum = new List<string>();
            message.Headers.AllOf = new List<Schema>();

            var sut = new AsyncApiContractResolver();

            var actual = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ContractResolver = sut,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

            Assert.Equal(expected, actual);
        }

        private Message GetMockMessage()
        {
            var payload = new Object
            {
                { "foo_Array", new Array { (String)"bar" } },
                { "foo_Array_empty", new Array() },
                { "foo_Array_with_null", new Array { Null.Instance } },
            };

            var headers = new Schema
            {
                Enum = new List<string> { "on", "off" },
                Required = new HashSet<string> { "Required" },
            };
            headers.AllOf.Add(new Schema());
            headers.AnyOf = new List<Schema>();

            var message = new Message
            {
                Name = "foo",
                Headers = headers,
                Payload = payload,
            };

            return message;
        }
    }
}