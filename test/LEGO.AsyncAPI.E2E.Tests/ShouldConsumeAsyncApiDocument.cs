using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests
{
    public class JsonPropertyAsyncApiDocument
    {
        private IReader<AsyncApiDocument> _asyncApiReader;
        private const string SampleFolderPath = "readers/samples/AsyncApi/";

        public JsonPropertyAsyncApiDocument()
        {
            _asyncApiReader = new AsyncApiReaderNewtonJson<AsyncApiDocument>();
        }

        [Fact]
        public async void JsonPropertyMinimalSpec()
        {
            var output = _asyncApiReader.Consume(Helper.ReadFileToStream(typeof(JsonPropertyAsyncApiDocument), SampleFolderPath, "Minimal.json"));
        
            Assert.Equal("2.3.0", output.AsyncApi);
            Assert.Equal("foo", output.Info.Title);
            Assert.Equal("bar", output.Info.Version);
        }
    }
}