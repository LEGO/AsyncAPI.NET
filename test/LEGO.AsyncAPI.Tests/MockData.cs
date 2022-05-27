namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
    using LEGO.AsyncAPI.Models.Interfaces;

    public static class MockData
    {
        public static Dictionary<string, IAsyncApiAny> Extensions()
        {
            return new Dictionary<string, IAsyncApiAny>
            {
                { "x-ext-null", AsyncAPINull.Instance },
                { "x-ext-integer", (AsyncAPILong)13 },
                { "x-ext-number", (AsyncAPIDouble)13.13 },
                { "x-ext-string", (AsyncAPIString)"bar" },
                { "x-ext-boolean", (AsyncAPIBoolean)true },
                { "x-ext-array", new AsyncAPIArray() { (AsyncAPIString)"foo", new AsyncAPIObject { ["foo"] = (AsyncAPIString)"bar" } } },
                { "x-ext-object", new AsyncAPIObject { ["foo"] = (AsyncAPIString)"bar" } },
            };
        }

        public static AsyncAPIObject Payload()
        {
            var payload = new AsyncAPIObject();
            payload.Add("foo", (AsyncAPIString)"bar");
            payload.Add("baz", (AsyncAPILong)13);
            payload.Add("bazz", (AsyncAPIDouble)13.13);
            payload.Add("grault", new AsyncAPIObject
            {
                { "garply", (AsyncAPIString)"waldo" },
            });
            payload.Add("qux", new AsyncAPIArray { (AsyncAPIString)"foo" });
            payload.Add("quux", (AsyncAPIBoolean)true);
            payload.Add("quuz", AsyncAPINull.Instance);
            payload.Add("xyz", new AsyncAPIArray());
            return payload;
        }

        public static IDictionary<string, IOperationBinding> OperationBindings()
        {
            return new Dictionary<string, IOperationBinding>()
            {
                {
                    "kafka", new KafkaOperationBinding
                    {
                        GroupId = new Schema(),
                        ClientId = new Schema(),
                        BindingVersion = "quz",
                        Extensions = new Dictionary<string, IAsyncApiAny>
                        {
                            {
                                "x-ext-string", new AsyncAPIString("foo")
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
                        Extensions = new Dictionary<string, IAsyncApiAny>
                        {
                            {
                                "x-ext-string", new AsyncAPIString("foo")
                            },
                        },
                    }
                },
            };
        }

        public static IDictionary<string, IMessageBinding> MessageBindings()
        {
            return new Dictionary<string, IMessageBinding>()
            {
                {
                    "http", new HttpMessageBinding()
                    {
                        Extensions = new Dictionary<string, IAsyncApiAny>
                        {
                            {
                                "x-ext-string", new AsyncAPIString("foo")
                            },
                        },
                    }
                },
            };
        }

        public static IDictionary<string, IChannelBinding> ChannelBindings()
        {
            return new Dictionary<string, IChannelBinding>()
            {
                {
                    "kafka",
                    new KafkaChannelBinding
                    {
                        Extensions = new Dictionary<string, IAsyncApiAny>
                        {
                            {
                                "x-ext-string", new AsyncAPIString("foo")
                            }
                        }
                    }
                },
            };
        }

        public static IDictionary<string, IServerBinding> ServerBindings()
        {
            return new Dictionary<string, IServerBinding>
            {
                {
                    "kafka",
                    new KafkaServerBinding
                    {
                        Extensions = new Dictionary<string, IAsyncApiAny>
                        {
                            {
                                "x-ext-string", new AsyncAPIString("foo")

                            }
                        }
                    }
                },
            };
        }

        public static Info Info(Contact? contact = null, License? license = null, bool withExtensionData = true)
        {
            return new Info("foo", "bar")
            {
                Description = "quz",
                TermsOfService = new Uri("https://lego.com"),
                Contact = contact ?? new Contact(),
                License = license ?? new License("Apache 2.0"),
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static Contact Contact(bool withExtensionData = true)
        {
            return new Contact
            {
                Name = "foo",
                Uri = new Uri("https://lego.com"),
                Email = "asyncApiContactObject@lego.com",
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static License License(bool withExtensionData = true)
        {
            return new License("Apache 2.0")
            {
                Url = new Uri("https://lego.com"),
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static Server Server(IDictionary<string, ServerVariable>? variables = null, IList<Dictionary<string, string[]>>? security = null, bool withExtensionData = true)
        {
            return new Server("https://lego.com", "http")
            {
                ProtocolVersion = "0.0.1",
                Description = "foo",
                Variables = variables ?? ImmutableDictionary<string, ServerVariable>.Empty,
                Security = security ?? new List<Dictionary<string, string[]>> { new() },
                Bindings = ServerBindings(),
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        private static Dictionary<string, IAsyncApiAny>? WithExtensionData(bool withExtensionData)
        {
            return withExtensionData ? Extensions() : null;
        }

        public static ServerVariable ServerVariable(bool withExtensionData = true)
        {
            return new ServerVariable()
            {
                Enum = new List<string> { "foo" },
                Default = "bar",
                Description = "baz",
                Examples = new List<string> { "quz" },
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static Channel Channel(bool withExtensionData = true)
        {
            return new Channel()
            {
                Description = "foo",
                Servers = new[] { "production" },
                Subscribe = new Operation(),
                Publish = new Operation(),
                Parameters = ImmutableDictionary<string, Parameter>.Empty,
                Bindings = ChannelBindings(),
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static Tag Tag(bool withExtensionData = true)
        {
            return new Tag()
            {
                Name = "foo",
                Description = "bar",
                ExternalDocs = new ExternalDocumentation(),
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static ExternalDocumentation ExternalDocs(bool withExtensionData = true)
        {
            return new ExternalDocumentation()
            {
                Description = "foo",
                Url = new Uri("https://lego.com"),
                Extensions = WithExtensionData(withExtensionData),
            };
        }

        public static Components Components(bool withExtensionData = true)
        {
            return new Components
            {
                OperationBindings = OperationBindings(),
                MessageBindings = MessageBindings(),
                ChannelBindings = ChannelBindings(),
                ServerBindings = ServerBindings(),
                Extensions = WithExtensionData(withExtensionData),
            };
        }
    }
}
