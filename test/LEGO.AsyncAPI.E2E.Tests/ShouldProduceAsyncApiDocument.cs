using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests
{
    public class ShouldProduceAsyncApiDocument
    {
        private const string SampleFolderPath = "readers/samples/AsyncApi/";
        
        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            var input = Helper.ReadFileToStreamAsString(typeof(ShouldProduceAsyncApiDocument), SampleFolderPath, "Minimal.json");
            
            Assert.Equal(input, new AsyncApiWriter<AsyncApiDocument>().Produce(new AsyncApiDocument{ AsyncApi = "2.3.0", Info = new Info { Title = "foo", Version = "bar" } }));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            var input = Helper.ReadFileToStreamAsString(typeof(ShouldProduceAsyncApiDocument), SampleFolderPath, "Complete.json");
            
            Assert.Equal(input, new AsyncApiWriter<AsyncApiDocument>().Produce(new AsyncApiDocument
            {
                AsyncApi = "2.3.0",
                Id = "urn:com:smartylighting:streetlights:server",
                Info = new Info
                {
                    Title = "foo", Version = "bar"
                }
            }));
        }
    }
}