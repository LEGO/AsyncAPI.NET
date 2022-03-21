namespace LEGO.AsyncAPI.Tests.Resolvers
{
    using System.Collections.Generic;
    using AsyncAPI.Models;
    using AsyncAPI.Models.Any;
    using Newtonsoft.Json;
    using NewtonUtils;
    using Xunit;

    public class AsyncApiContractResolver_Should
    {
        [Fact]
        public void NotSerializeEmptyArray()
        {
            // Arrange
            var expected = @"{
  ""$id"": ""1"",
  ""foo"": ""bar"",
  ""baz"": 13,
  ""bazz"": 13.13,
  ""grault"": {
    ""$id"": ""2"",
    ""garply"": ""waldo"",
    ""non_empty_array"": [
      ""xyz""
    ],
    ""quux"": true
  }
}";
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

            // Act
            var actual = JsonConvert.SerializeObject(@object, JsonSerializerUtils.Settings);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeMessageWithNonEmptyArrayHeaders_ThenArraysSerialized()
        {
            // Arrange
            var expected = @"{
  ""$id"": ""1"",
  ""headers"": {
    ""$id"": ""2"",
    ""allOf"": [
      {
        ""$id"": ""3""
      }
    ],
    ""required"": [
      ""Required""
    ],
    ""enum"": [
      ""on"",
      ""off""
    ]
  },
  ""payload"": {
    ""foo_Array"": [
      ""bar""
    ],
    ""foo_Array_empty"": [],
    ""foo_Array_with_null"": [
      null
    ]
  },
  ""name"": ""foo""
}";

            var message = GetMockMessage();

            // Act
            var actual = JsonConvert.SerializeObject(message, JsonSerializerUtils.Settings);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeMessageWithEmptyArrayHeaders_ThenArraysNotSerialized()
        {
            // Arrange
            var expected = @"{
  ""$id"": ""1"",
  ""headers"": {
    ""$id"": ""2""
  },
  ""payload"": {
    ""foo_Array"": [
      ""bar""
    ],
    ""foo_Array_empty"": [],
    ""foo_Array_with_null"": [
      null
    ]
  },
  ""name"": ""foo""
}";

            var message = GetMockMessage();
            message.Headers.Required = new HashSet<string>();
            message.Headers.Enum = new List<string>();
            message.Headers.AllOf = new List<Schema>();

            // Act
            var actual = JsonConvert.SerializeObject(message, JsonSerializerUtils.Settings);

            // Assert
            Assert.Equal(expected, actual);
        }

        private Message GetMockMessage()
        {
            var payload = new AsyncAPIObject
            {
                { "foo_Array", new AsyncAPIArray { (AsyncAPIString)"bar" } },
                { "foo_Array_empty", new AsyncAPIArray() },
                { "foo_Array_with_null", new AsyncAPIArray { AsyncAPINull.Instance } },
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