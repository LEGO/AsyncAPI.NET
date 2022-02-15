using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiInfoObject
{
    public class ShouldConsumeInfo : ShouldConsumeProduceBase<Info>
    {
        public ShouldConsumeInfo() : base(typeof(ShouldConsumeInfo))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
            Assert.Equal("quz", output.Description);
            Assert.Equal(new Uri("https://lego.com"), output.TermsOfService);
            Assert.IsType<Contact>(output.Contact);
            Assert.Equal(new List<License> {new("Apache 2.0"), new("Apache 2.0")}, output.License);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}