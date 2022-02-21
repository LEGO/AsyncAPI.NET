using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiServerVariableObject
{
    public class ShouldConsumeServerVariable : ShouldConsumeProduceBase<ServerVariable>
    {
        public ShouldConsumeServerVariable() : base(typeof(ShouldConsumeServerVariable))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.IsType<ServerVariable>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = _asyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal(new List<string> {"foo"}, output.Enum);
            Assert.Equal("bar", output.Default);
            Assert.Equal("baz", output.Description);
            Assert.Equal(new List<string> {"quz"}, output.Examples);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}