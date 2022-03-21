namespace LEGO.AsyncAPI.Tests
{
    using System.IO;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Xunit;

    public class PayloadConverterReadJsonTest
    {
        [Fact]
        public void ShouldMapNull()
        {
            var output = GetOutputFor<AsyncAPINull>("null");
            Assert.Null(output);
        }

        [Fact]
        public void ShouldConsumeObject()
        {
            var output = GetOutputForClass<AsyncAPIObject>("{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\"},\"qux\":[],\"quux\":true,\"quuz\":null}");
            Assert.NotNull(output);
            Assert.IsType<AsyncAPIObject>(output);
            Assert.IsType<AsyncAPIString>(output?["foo"]);
            Assert.IsType<AsyncAPILong>(output?["baz"]);
            Assert.IsType<AsyncAPIDouble>(output?["bazz"]);
            Assert.IsType<AsyncAPIArray>(output?["qux"]);
            Assert.IsType<AsyncAPIBoolean>(output?["quux"]);
            Assert.Null(output["quuz"]);
            Assert.IsType<AsyncAPIObject>(output?["grault"]);
            var grault = output?["grault"] as AsyncAPIObject;
            Assert.IsType<AsyncAPIString>(grault?["garply"]);
        }

        [Fact]
        public void ShouldConsumeString()
        {
            var output = GetOutputFor<AsyncAPIString>("\"foo\"");

            Assert.IsType<AsyncAPIString>(output);
            Assert.Equal("foo", output?.Value);
        }

        [Fact]
        public void ShouldConsumeDouble()
        {
            var output = GetOutputFor<AsyncAPIDouble>("13.13");

            Assert.IsType<AsyncAPIDouble>(output);
            Assert.Equal(13.13, output?.Value);
        }

        [Fact]
        public void ShouldConsumeLong()
        {
            var output = GetOutputFor<AsyncAPILong>("134341421");

            Assert.IsType<AsyncAPILong>(output);
            Assert.Equal(134341421, output?.Value);
        }

        [Fact]
        public void ShouldConsumeArray()
        {
            var output = GetOutputForClass<AsyncAPIArray>("[ \"foo\", \"bar\", 13.13, {} ]");

            Assert.IsType<AsyncAPIArray>(output);
            Assert.Equal(4, output.Count);
            Assert.Equal("foo", ((AsyncAPIString)output[0]).Value);
            Assert.Equal(13.13, ((AsyncAPIDouble)output[2]).Value);
            Assert.IsType<AsyncAPIObject>(output[3]);
        }

        [Fact]
        public void ShouldConsumeBoolean()
        {
            var outputTrue = GetOutputFor<AsyncAPIBoolean>("true");

            Assert.IsType<AsyncAPIBoolean>(outputTrue);
            Assert.NotNull(outputTrue);
            Assert.True(outputTrue?.Value);

            var outputFalse = GetOutputFor<AsyncAPIBoolean>("false");

            Assert.IsType<AsyncAPIBoolean>(outputFalse);
            Assert.NotNull(outputTrue);
            Assert.False(outputFalse?.Value);
        }

        private static T GetOutputFor<T>(string input)
            where T : class
        {
            var jsonTextReader = new JsonTextReader(new StringReader(input));
            jsonTextReader.Read();
            return (T)new AnyConverter().ReadJson(jsonTextReader, typeof(T), null, JsonSerializerUtils.Serializer);
        }

        private static T GetOutputForClass<T>(string input)
            where T : class
        {
            var jsonTextReader = new JsonTextReader(new StringReader(input));
            jsonTextReader.Read();
            return (T)new AnyConverter().ReadJson(jsonTextReader, typeof(T), null, JsonSerializerUtils.Serializer);
        }
    }
}
