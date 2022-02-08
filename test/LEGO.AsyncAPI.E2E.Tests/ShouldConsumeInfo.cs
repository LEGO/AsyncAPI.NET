using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests
{
    public class JsonPropertyInfo
    {
        private IReader<Info> _asyncApiReader;
        private const string SampleFolderPath = "readers/samples/AsyncApi/AsyncApiInfoObject/";

        public JsonPropertyInfo()
        {
            _asyncApiReader = new AsyncApiReaderNewtonJson<Info>();
        }

        [Fact]
        public void JsonPropertyMinimalJsonSpec()
        {
            var stream = Helper.ReadFileToStream(typeof(JsonPropertyInfo), SampleFolderPath, "Minimal.json");
            var output = _asyncApiReader.Consume(stream);
        
            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
        }
        
        [Fact]
        public void JsonPropertyCompleteSpec()
        {
            var stream = Helper.ReadFileToStream(typeof(JsonPropertyInfo), SampleFolderPath, "Complete.json");
            var output = _asyncApiReader.Consume(stream);
            
            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
            Assert.Equal("quz", output.Description);
            Assert.Equal(new Uri("https://lego.com"), output.TermsOfService);
            Assert.IsType<Contact>(output.Contact);
            Assert.Equal(new List<License>{ new() { Name = "Apache 2.0"}, new() { Name = "Apache 2.0"} }, output.License);
        }
    }
}