namespace LEGO.AsyncAPI.E2E.Tests.Readers.Samples.AsyncApi.AsyncApiOperationTraitObject
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Tests;
    using Xunit;

    public class ShouldProduceOperationTrait : ShouldConsumeProduceBase<OperationTrait>
    {
        public ShouldProduceOperationTrait()
            : base(typeof(ShouldProduceOperationTrait))
        {
        }

        [Fact]
        public void ShouldProduceMinimalSpec()
        {
            Assert.Equal(this.GetString("Minimal.json"), this.AsyncApiWriter.Write(new OperationTrait()));
        }

        [Fact]
        public void ShouldProduceCompleteSpec()
        {
            Assert.Equal(this.GetStringWithMockedExtensions("Complete.json"), this.AsyncApiWriter.Write(new OperationTrait
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
                                    "x-ext-string", new AsyncAPIString()
                                    {
                                        Value = "foo",
                                    }
                                },
                            },
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
                                    "x-ext-string", new AsyncAPIString()
                                    {
                                        Value = "foo",
                                    }
                                },
                            },
                        }
                    },
                },
                Extensions = MockData.Extensions(),
            }));
        }
    }
}