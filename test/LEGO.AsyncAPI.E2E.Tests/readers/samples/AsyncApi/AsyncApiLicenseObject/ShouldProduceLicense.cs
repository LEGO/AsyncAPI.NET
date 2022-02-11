using System;
using LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiInfoObject;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiLicenseObject
{
    public class ShouldProduceLicense: ShouldConsumeProduceBase<License>
    {
        public ShouldProduceLicense(): base(typeof(ShouldProduceLicense))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Produce(new License("Apache 2.0")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Produce(new License("Apache 2.0")
            {
                Url = new Uri("https://lego.com"),
                Extensions = MockData.Extensions()
            }));
        }
    }
}