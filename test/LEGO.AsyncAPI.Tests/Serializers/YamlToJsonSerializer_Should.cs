namespace LEGO.AsyncAPI.Tests.Serializers
{
    using System.Linq;
    using LEGO.AsyncAPI.Readers.Serializers;
    using Newtonsoft.Json.Linq;
    using Xunit;
    using YamlDotNet.Core;

    public class YamlToJsonSerializerShould
    {
        private readonly YamlToJsonSerializer sut;

        private const string Json =
            "{\"List\": [\"a\", \"b\"], \"xyz\": \"xyz_value\", \"foo\": true, \"bar\": 100, \"baz\": 111.1}";

        public YamlToJsonSerializerShould()
        {
            this.sut = new YamlToJsonSerializer();
        }

        [Fact]
        public void Serialize_Yaml_ThenReturnsJsonWithCorrectPrimitiveTypes()
        {
            // Arrange
            var input = @"
List:
  - a
  - b
xyz: xyz_value
foo: true
bar: 100
baz: 111.1
";

            // Act
            var result = this.sut.Serialize(input);

            // Assert
            this.VerifyResult(result);
        }

        [Fact]
        public void Serialize_Json_ThenReturnsJsonWithCorrectPrimitiveTypes()
        {
            // Act
            var result = this.sut.Serialize(Json);

            // Assert
            this.VerifyResult(result);
        }

        [Fact]
        public void Serialize_InvalidYamlFormat_ThenThrows()
        {
            // Arrange
            var input = @"
foo:  
 xyz:
xyz_value
";

            // Act
            // Assert
            Assert.Throws<SemanticErrorException>(() => this.sut.Serialize(input));
        }

        private void VerifyResult(string json)
        {
            Assert.Equal(Json, json.TrimEnd());

            var jObject = JObject.Parse(json);

            Assert.True(jObject.HasValues);
            Assert.True(jObject.ContainsKey("xyz"));
            Assert.True(jObject.ContainsKey("foo"));
            Assert.True(jObject.ContainsKey("bar"));
            Assert.True(jObject.ContainsKey("baz"));
            Assert.Equal("xyz_value", jObject["xyz"]?.Value<string>());
            Assert.Equal(true, jObject["foo"]?.Value<bool>());
            Assert.Equal(100, jObject["bar"]?.Value<int>());
            Assert.Equal(111.1, jObject["baz"]?.Value<double>());
            Assert.Equal(2, jObject["List"]?.Values().Count());
        }
    }
}