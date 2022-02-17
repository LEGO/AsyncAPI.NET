using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiTagObject
{
    public class ShouldProduceTag: ShouldConsumeProduceBase<Tag>
    {
        public ShouldProduceTag(): base(typeof(ShouldProduceTag))
        {
        }

        [Fact]
        public async void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Tag()));
        }
        
        [Fact]
        public async void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(MockData.Tag()));
        }
    }
}