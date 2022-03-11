namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiLicenseObject
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Xunit;

    public class ShouldConsumeLicense : ShouldConsumeProduceBase<License>
    {
        public ShouldConsumeLicense() : base(typeof(ShouldConsumeLicense))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.Equal("Apache 2.0", output.Name);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("Apache 2.0", output.Name);
            Assert.Equal(new Uri("https://lego.com"), output.Url);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}