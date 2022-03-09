namespace LEGO.AsyncAPI.Tests
{
    using System.IO;
    using AsyncAPI.Models.Any;
    using Converters;
    using Newtonsoft.Json;
    using NewtonUtils;
    using Xunit;

    public class PayloadConverterWriteJsonTest
    {
        [Fact]
        public void ShouldProduceNull()
        {
            Assert.Equal("null", GetOutputFor(Null.Instance));
        }

        [Fact]
        public void ShouldConsumeObject()
        {
            Object obj = new Object();
            obj.Add("foo", new String(){Value = "bar"});
            obj.Add("baz", new Long(value: 13));
            obj.Add("bazz", new Double(value: 13.13));
            var grault = new Object();
            grault.Add("garply", new String() { Value = "waldo"});
            obj.Add("grault", grault);
            obj.Add("qux", new Array());
            obj.Add("quux", new Boolean(value: true));
            obj.Add("quuz", Null.Instance);
            var output = GetOutputFor(obj);
            Assert.Equal("{\"foo\":\"bar\",\"baz\":13,\"bazz\":13.13,\"grault\":{\"garply\":\"waldo\"},\"qux\":[],\"quux\":true,\"quuz\":null}", output);
        }
    
        [Fact]
        public void ShouldProduceString()
        {
            var output = GetOutputFor(new String(){Value = "foo"});
        
            Assert.Equal("\"foo\"", output);
        }
    
        [Fact]
        public void ShouldProduceDouble()
        {
            var output = GetOutputFor(new Double(value: 13.13));
        
            Assert.Equal("13.13", output);
        }
    
        [Fact]
        public void ShouldProduceLong()
        {
            var output = GetOutputFor(new Long(value: 134341421));
        
            Assert.Equal("134341421", output);
        }
    
        [Fact]
        public void ShouldProduceArray()
        {
            var output = GetOutputFor(new Array { 
                new String {Value = "foo"},
                new String {Value = "bar"},
                new Double(value: 13.13),
                new Object ()
            });
        
            Assert.Equal("[\"foo\",\"bar\",13.13,{}]", output);
        }
    
        [Fact]
        public void ShouldProduceBoolean()
        {
            var output = GetOutputFor(new Boolean(value: true));
        
            Assert.Equal("true", output);
        }

        private static string GetOutputFor(IAny input)
        {
            var converter = new AnyConverter();
            var stringWriter = new StringWriter();
            JsonWriter jsonTextWriter = new JsonTextWriter(stringWriter);
            converter.WriteJson(jsonTextWriter, input, JsonSerializerUtils.Serializer);
            return stringWriter.ToString();
        }
    }
}
