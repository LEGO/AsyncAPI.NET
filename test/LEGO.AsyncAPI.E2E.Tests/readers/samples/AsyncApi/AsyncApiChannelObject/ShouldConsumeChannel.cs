// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiChannelObject
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Xunit;

    public class ShouldConsumeChannel : ShouldConsumeProduceBase<Channel>
    {
        public ShouldConsumeChannel()
            : base(typeof(ShouldConsumeChannel))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStream("Minimal.json"));

            Assert.IsType<Channel>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = this.AsyncApiAsyncApiReader.Read(this.GetStreamWithMockedExtensions("Complete.json"));

            Assert.Equal("foo", output.Description);
            Assert.IsAssignableFrom<IList<string>>(output.Servers);
            Assert.IsType<Operation>(output.Subscribe);
            Assert.IsType<Operation>(output.Publish);
            Assert.IsAssignableFrom<IDictionary<string, Parameter>>(output.Parameters);
            Assert.IsAssignableFrom<IDictionary<string, IChannelBinding>>(output.Bindings);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}