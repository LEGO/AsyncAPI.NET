using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOperationObject
{
    public class ShouldProduceOperation : ShouldConsumeProduceBase<Operation>
    {
        public ShouldProduceOperation() : base(typeof(ShouldProduceOperation))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new Operation()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new Operation
            {
                OperationId = "foo",
                Summary = "bar",
                Description = "baz",
                Tags = ImmutableList<Tag>.Empty,
                ExternalDocs = new ExternalDocumentation(),
                Bindings = new Dictionary<string, IOperationBinding>()
                {
                    {
                        "kafka", new KafkaOperationBinding
                        {
                            GroupId = new Schema(),
                            ClientId = new Schema(),
                            BindingVersion = "quz",
                            Extensions = new Dictionary<string, IAny>
                            {
                                {
                                    "x-ext-string", new String()
                                    {
                                        Value = "foo"
                                    }
                                }
                            }
                        }
                    },
                    {
                        "http", new HttpOperationBinding
                        {
                            Type = "request",
                            Method = "GET",
                            Query = new Schema(),
                            BindingVersion = "quz",
                            Extensions = new Dictionary<string, IAny>
                            {
                                {
                                    "x-ext-string", new String()
                                    {
                                        Value = "foo"
                                    }
                                }
                            }
                        }
                    }

                },
                Traits = ImmutableList<OperationTrait>.Empty,
                Message = new Dictionary<string, List<Message>>()
                {
                    { "oneOf", new List<Message>() {new()}}
                },
                Extensions = MockData.Extensions()
            }));
        }
    }

    public class KafkaBinding : IChannelBinding
    {
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}