namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiContactObject
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeContact : ShouldConsumeProduceBase<Contact>
    {
        public ShouldConsumeContact()
            : base(typeof(ShouldConsumeContact))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.IsType<Contact>(this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.Name);
            Assert.Equal(new Uri("https://lego.com"), output.Uri);
            Assert.Equal("asyncApiContactObject@lego.com", output.Email);
            var extensions = output.Extensions;
            Assert.IsAssignableFrom<IDictionary<string, IAsyncApiAny>>(extensions);
            var extension = (AsyncAPIString)extensions["x-ext-string"];
            Assert.Equal("bar", (string)extension!);
        }
    }
}