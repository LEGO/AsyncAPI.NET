using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiContactObject;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiInfoObject
{
    public class ShouldProduceInfo: ShouldConsumeProduceBase<Info>
    {
        public ShouldProduceInfo(): base(typeof(ShouldProduceInfo))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Info("foo", "bar")));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            var license = new License("Apache 2.0");
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new Info("foo", "bar")
            {
                Description = "quz",
                TermsOfService = new Uri("https://lego.com"),
                Contact = new Contact(),
                License = new List<License>()
                {
                    license,
                    license
                },
                Extensions = MockData.Extensions()
            }));
        }
    }
}