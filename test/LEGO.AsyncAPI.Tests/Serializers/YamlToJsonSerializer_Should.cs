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

        public YamlToJsonSerializerShould()
        {
            this.sut = new YamlToJsonSerializer();
        }

        [Fact]
        public void Serialize_Yaml_ThenReturnsCorrectJsonString()
        {
            // Arrange
            var input = @"
List:
  - a
  - b
xyz: xyz_value
foo: foo_value
";

            // Act
            var result = this.sut.Serialize(input);

            // Assert
            this.VerifyResult(result);
        }

        [Fact]
        public void Serialize_Json_ThenReturnsCorrectJsonString()
        {
            // Arrange
            var input = "{\"List\":[\"a\",\"b\"], \"xyz\":\"xyz_value\", \"foo\":\"foo_value\"}";

            // Act
            var result = this.sut.Serialize(input);

            // Assert
            this.VerifyResult(result);
        }

        [Fact]
        public void Serialize_InvalidFormat_ThenThrows()
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
            var jObject = JObject.Parse(json);

            Assert.True(jObject.HasValues);
            Assert.True(jObject.ContainsKey("xyz"));
            Assert.True(jObject.ContainsKey("foo"));
            Assert.Equal("xyz_value", jObject["xyz"]?.Value<string>());
            Assert.Equal("foo_value", jObject["foo"]?.Value<string>());
            Assert.Equal(2, jObject["List"]?.Values().Count());
        }
    }
}