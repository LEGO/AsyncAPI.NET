using System;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiExternalDocsObject
{
    public class ShouldProduceExternalDocs: ShouldConsumeProduceBase<ExternalDocumentation>
    {
        public ShouldProduceExternalDocs(): base(typeof(ShouldProduceExternalDocs))
        {
        }

        [Fact]
        public async void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new ExternalDocumentation()));
        }
        
        [Fact]
        public async void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.ExternalDocs()));
        }
    }
}