using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiServerVariableObject
{
    public class ShouldProduceServerVariable : ShouldConsumeProduceBase<ServerVariable>
    {
        public ShouldProduceServerVariable() : base(typeof(ShouldProduceServerVariable))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Produce(new ServerVariable()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Produce(new ServerVariable()
            {
                Enum = new List<string> {"foo"},
                Default = "bar",
                Description = "baz",
                Examples = new List<string> {"quz"},
                Extensions = MockData.Extensions()
            }));
        }
    }
}