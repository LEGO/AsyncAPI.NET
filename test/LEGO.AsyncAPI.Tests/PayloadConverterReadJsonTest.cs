namespace LEGO.AsyncAPI.Tests
{
    using System.IO;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Xunit;

    public class PayloadConverterReadJsonTest
    {
        [Fact]
        public void ShouldMapNull()
        {
            Assert.IsType<Null>(GetOutputFor<Null>("null"));
        }

        [Fact]
        public void ShouldConsumeObject()
        {
            var output = GetOutputForClass<Object>("{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\"},\"qux\":[],\"quux\":true,\"quuz\":null}");
            Assert.IsType<Object>(output);
            Assert.IsType<String>(output?["foo"]);
            Assert.IsType<Long>(output?["baz"]);
            Assert.IsType<Double>(output?["bazz"]);
            Assert.IsType<Array>(output?["qux"]);
            Assert.IsType<Boolean>(output?["quux"]);
            Assert.IsType<Null>(output?["quuz"]);
            Assert.IsType<Object>(output?["grault"]);
            var grault = output?["grault"] as Object;
            Assert.IsType<String>(grault?["garply"]);
        }

        [Fact]
        public void ShouldConsumeString()
        {
            var output = GetOutputFor<String>("\"foo\"");

            Assert.IsType<String>(output);
            Assert.Equal("foo", output?.Value);
        }

        [Fact]
        public void ShouldConsumeDouble()
        {
            var output = GetOutputFor<Double>("13.13");

            Assert.IsType<Double>(output);
            Assert.Equal(13.13, output?.Value);
        }

        [Fact]
        public void ShouldConsumeLong()
        {
            var output = GetOutputFor<Long>("134341421");

            Assert.IsType<Long>(output);
            Assert.Equal(134341421, output?.Value);
        }

        [Fact]
        public void ShouldConsumeArray()
        {
            var output = GetOutputForClass<Array>("[ \"foo\", \"bar\", 13.13, {} ]");

            Assert.IsType<Array>(output);
            Assert.Equal(4, output.Count);
            Assert.Equal("foo", ((String)output[0]).Value);
            Assert.Equal(13.13, ((Double)output[2]).Value);
            Assert.IsType<Object>(output[3]);
        }

        [Fact]
        public void ShouldConsumeBoolean()
        {
            var outputTrue = GetOutputFor<Boolean>("true");

            Assert.IsType<Boolean>(outputTrue);
            Assert.True(outputTrue.HasValue && outputTrue.Value.Value);

            var outputFalse = GetOutputFor<Boolean>("false");

            Assert.IsType<Boolean>(outputFalse);
            Assert.False(outputFalse != null && outputFalse.Value.Value);
        }

        private static T? GetOutputFor<T>(string input)
            where T : struct
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
