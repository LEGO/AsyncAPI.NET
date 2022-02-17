using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
using LEGO.AsyncAPI.Tests;
using Xunit;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi.AsyncApiOperationTraitObject
{
    public class ShouldProduceOperationTrait : ShouldConsumeProduceBase<OperationTrait>
    {
        public ShouldProduceOperationTrait() : base(typeof(ShouldProduceOperationTrait))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(GetString("Minimal.json"), _asyncApiWriter.Write(new OperationTrait()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(GetStringWithMockedExtensions("Complete.json"), _asyncApiWriter.Write(new OperationTrait
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
                Extensions = MockData.Extensions()
            }));
        }
    }
}