namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiInfoObject
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Xunit;

    public class ShouldConsumeInfo : ShouldConsumeProduceBase<Info>
    {
        public ShouldConsumeInfo() : base(typeof(ShouldConsumeInfo))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.Title);
            Assert.Equal("bar", output.Version);
            Assert.Equal("quz", output.Description);
            Assert.Equal(new Uri("https://lego.com"), output.TermsOfService);
            Assert.IsType<Contact>(output.Contact);
            Assert.Equal(new License("Apache 2.0"), output.License);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}