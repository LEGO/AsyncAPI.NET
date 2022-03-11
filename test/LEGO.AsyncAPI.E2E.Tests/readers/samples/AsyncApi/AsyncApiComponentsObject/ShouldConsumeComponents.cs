namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiComponentsObject
{
    using System.Collections.Generic;
    using Models;
    using Models.Any;
    using Models.Interfaces;
    using Xunit;

    public class ShouldConsumeComponents : ShouldConsumeProduceBase<Components>
    {
        public ShouldConsumeComponents() : base(typeof(ShouldConsumeComponents))
        {
        }

        [Fact]
        public void ShouldConsumeMinimalSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStream("Minimal.json"));

            Assert.IsType<Components>(output);
        }

        [Fact]
        public void ShouldConsumeCompleteSpec()
        {
            var output = AsyncApiAsyncApiReader.Read(GetStreamWithMockedExtensions("Complete.json"));

            Assert.IsAssignableFrom<IDictionary<string, Schema>>(output.Schemas);
            Assert.IsAssignableFrom<IDictionary<string, Server>>(output.Servers);
            Assert.IsAssignableFrom<IDictionary<string, Channel>>(output.Channels);
            Assert.IsAssignableFrom<IDictionary<string, Message>>(output.Messages);
            Assert.IsAssignableFrom<IDictionary<string, SecurityScheme>>(output.SecuritySchemes);
            Assert.IsAssignableFrom<IDictionary<string, Parameter>>(output.Parameters);
            Assert.IsAssignableFrom<IDictionary<string, CorrelationId>>(output.CorrelationIds);
            Assert.IsAssignableFrom<IDictionary<string, OperationTrait>>(output.OperationTraits);
            Assert.IsAssignableFrom<IDictionary<string, MessageTrait>>(output.MessageTraits);
            Assert.IsAssignableFrom<IDictionary<string, IServerBinding>>(output.ServerBindings);
            Assert.IsAssignableFrom<IDictionary<string, IChannelBinding>>(output.ChannelBindings);
            Assert.IsAssignableFrom<IDictionary<string, IOperationBinding>>(output.OperationBindings);
            Assert.IsAssignableFrom<IDictionary<string, IMessageBinding>>(output.MessageBindings);
            Assert.IsAssignableFrom<IDictionary<string, IAny>>(output.Extensions);
        }
    }
}