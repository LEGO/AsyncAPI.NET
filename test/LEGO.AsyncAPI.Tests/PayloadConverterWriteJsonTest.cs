namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.IO;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Xunit;

    public class PayloadConverterWriteJsonTest
    {
        [Fact]
        public void ShouldProduceNull()
        {
            Assert.Equal("null", GetOutputFor(AsyncAPINull.Instance));
        }

        [Fact]
        public void ShouldConsumeObject()
        {
            AsyncAPIObject obj = new AsyncAPIObject();
            obj.Add("foo", new AsyncAPIString("bar"));
            obj.Add("baz", new AsyncAPILong(value: 13));
            obj.Add("bazz", new AsyncAPIDouble(value: 13.13));
            var grault = new AsyncAPIObject();
            grault.Add("garply", new AsyncAPIString("waldo"));
            obj.Add("grault", grault);
            obj.Add("qux", new AsyncAPIArray());
            obj.Add("quux", new AsyncAPIBoolean(value: true));
            obj.Add("quuz", AsyncAPINull.Instance);
            var output = GetOutputFor(obj);
            Assert.Equal("{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\"},\"qux\":[],\"quux\":true,\"quuz\":null}", output);
        }

        [Fact]
        public void ShouldProduceString()
        {
            var output = GetOutputFor(new AsyncAPIString("foo"));

            Assert.Equal("\"foo\"", output);
        }

        [Fact]
        public void ShouldProduceDouble()
        {
            var output = GetOutputFor(new AsyncAPIDouble(value: 13.13));

            Assert.Equal("13.13", output);
        }

        [Fact]
        public void ShouldProduceLong()
        {
            var output = GetOutputFor(new AsyncAPILong(value: 134341421));

            Assert.Equal("134341421", output);
        }

        [Fact]
        public void ShouldProduceArray()
        {
            var output = GetOutputFor(new AsyncAPIArray
            {
                new AsyncAPIString("foo"),
                new AsyncAPIString("bar"),
                new AsyncAPIDouble(value: 13.13),
                new AsyncAPIObject(),
            });

            Assert.Equal("[\"foo\",\"bar\",13.13,{}]", output);
        }

        [Fact]
        public void ShouldProduceBoolean()
        {
            var output = GetOutputFor(new AsyncAPIBoolean(value: true));

            Assert.Equal("true", output);
        }

        private static string GetOutputFor(IAsyncApiAny input)
        {
            var converter = new AnyConverter();
            var stringWriter = new StringWriter();
            JsonWriter jsonTextWriter = new JsonTextWriter(stringWriter);
            converter.WriteJson(jsonTextWriter, input, JsonSerializerUtils.Serializer);
            return stringWriter.ToString();
        }
    }
}
