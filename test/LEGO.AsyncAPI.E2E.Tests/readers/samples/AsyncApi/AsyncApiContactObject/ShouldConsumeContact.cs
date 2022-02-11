using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiContactObject
{
    public class ShouldConsumeContact : ShouldConsumeProduceBase<Contact>
    {
        public ShouldConsumeContact() : base(typeof(ShouldConsumeContact))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            Assert.IsType<Contact>(_asyncApiAsyncApiReader.Consume(GetStream("Minimal.json")));
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Consume(GetStream("Complete.json"));

            Assert.Equal("foo", output.Name);
            Assert.Equal(new Uri("https://lego.com"), output.Url);
            Assert.Equal("asyncApiContactObject@lego.com", output.Email);
            var extensions = output.Extensions;
            Assert.IsAssignableFrom<IDictionary<string, JToken>>(extensions);
            Assert.Equal("bar", extensions["x-ext-string"].Value<string>());
        }
    }
}