using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiExternalDocsObject
{
    public class ShouldConsumeExternalDocs: ShouldConsumeProduceBase<ExternalDocumentation>
    {
        public ShouldConsumeExternalDocs(): base(typeof(ShouldConsumeExternalDocs))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.NotNull(_asyncApiAsyncApiReader.Read(GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));
        
            Assert.Equal("foo", output.Description);
            Assert.Equal(new Uri("https://lego.com"), output.Url);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}