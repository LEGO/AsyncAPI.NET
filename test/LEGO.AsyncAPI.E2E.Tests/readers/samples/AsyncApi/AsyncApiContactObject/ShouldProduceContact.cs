using System;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiContactObject
{
    public class ShouldProduceContact: ShouldConsumeProduceBase<Contact>
    {
        public ShouldProduceContact(): base(typeof(ShouldProduceContact))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Produce(new Contact()));
        }
        
        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetString("Complete.json"), _asyncApiWriter.Produce(new Contact
            {
                Name = "foo",
                Url = new Uri("https://lego.com"),
                Email = "asyncApiContactObject@lego.com",
                Extensions = MockData.Extensions()
            }));
        }
    }
}