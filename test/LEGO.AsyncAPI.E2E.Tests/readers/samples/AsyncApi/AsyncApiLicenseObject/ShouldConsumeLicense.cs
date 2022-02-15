using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiLicenseObject
{
    public class ShouldConsumeLicense : ShouldConsumeProduceBase<License>
    {
        public ShouldConsumeLicense() : base(typeof(ShouldConsumeLicense))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.Equal("Apache 2.0", output.Name);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("Apache 2.0", output.Name);
            Assert.Equal(new Uri("https://lego.com"), output.Url);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}