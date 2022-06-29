namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiInfoObject
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeInfo : ShouldConsumeProduceBase<AsyncApiInfo>
    {
        public ShouldConsumeInfo()
            : base(typeof(ShouldConsumeInfo))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
            Assert.Equal("quz", output.Description);
            Assert.Equal(new Uri("https://lego.com"), output.TermsOfService);
            Assert.IsType<AsyncApiContact>(output.Contact);
            Assert.Equal(new AsyncApiLicense("Apache 2.0"), output.License);
            Assert.IsAssignableFrom<IDictionary<string, IAsyncApiAny>>(output.Extensions);
        }
    }
}