using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
using LEGO.AsyncAPI.Models.Interfaces;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Array = LEGO.AsyncAPI.Any.Array;
using Boolean = LEGO.AsyncAPI.Any.Boolean;
using Double = LEGO.AsyncAPI.Any.Double;
using Object = LEGO.AsyncAPI.Any.Object;
using String = LEGO.AsyncAPI.Any.String;

namespace LEGO.AsyncAPI.Tests;

public static class MockData
{
    public static Dictionary<string, IAny> Extensions()
    {
        return new Dictionary<string, IAny>
        {
            {"x-ext-null", new Null()},
            {"x-ext-integer", (Long) 13},
            {"x-ext-number", (Double) 13.13},
            {"x-ext-string", (String) "bar"},
            {"x-ext-boolean", (Boolean) true},
            {"x-ext-array", new Array() {(String) "foo", new Object {["foo"] = (String) "bar"}}},
            {"x-ext-object", new Object {["foo"] = (String) "bar"}}
        };
    }

    public static Object Payload()
    {
        var payload = new Object();
        payload.Add("foo", (String) "bar");
        payload.Add("baz", (Long) 13);
        payload.Add("bazz", (Double) 13.13);
        payload.Add("grault", new Object
        {
            {"garply", (String) "waldo"}
        });
        payload.Add("qux", new Array {(String) "foo"});
        payload.Add("quux", (Boolean) true);
        payload.Add("quuz", new Null());
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
        };
    }

    public static IDictionary<string, IMessageBinding> MessageBindings()
    {
        return new Dictionary<string, IMessageBinding>()
        {
            {
                "http", new HttpMessageBinding()
                {
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
        };
    }

    public static IDictionary<string, IChannelBinding> ChannelBindings()
    {
        return new Dictionary<string, IChannelBinding>()
        {
            {
                "kafka",
                new KafkaChannelBinding()
                    {Extensions = new Dictionary<string, IAny> {{"x-ext-string", new String() {Value = "foo"}}}}
            }
        };
    }

    public static IDictionary<string, IServerBinding> ServerBindings()
    {
        return new Dictionary<string, IServerBinding>
        {
            {
                "kafka",
                new KafkaServerBinding
                    {Extensions = new Dictionary<string, IAny> {{"x-ext-string", new String {Value = "foo"}}}}
            }
        };
    }

    public static Info Info(Contact? contact = null, License? license = null, bool withExtensionData = true)
    {
        return new Info("foo", "bar")
        {
            Description = "quz",
            TermsOfService = new Uri("https://lego.com"),
            Contact = contact??new Contact(),
            License = new List<License>()
            {
                license??new License("Apache 2.0")
            },
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    public static Contact Contact(bool withExtensionData = true)
    {
        return new Contact
        {
            Name = "foo",
            Uri = new Uri("https://lego.com"),
            Email = "asyncApiContactObject@lego.com",
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    public static License License(bool withExtensionData = true)
    {
        return new License("Apache 2.0")
        {
            Url = new Uri("https://lego.com"),
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    public static Server Server(IDictionary<string, ServerVariable>? variables = null, IDictionary<string, string[]>? security = null, bool withExtensionData = true)
    {
        return new Server("https://lego.com", "http")
        {
            ProtocolVersion = "0.0.1",
            Description = "foo",
            Variables = variables??ImmutableDictionary<string, ServerVariable>.Empty,
            Security = security??ImmutableDictionary<string, string[]>.Empty,
            Bindings = ServerBindings(),
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    private static Dictionary<string, IAny>? WithExtensionData(bool withExtensionData)
    {
        return withExtensionData ? Extensions() : null;
    }

    public static ServerVariable ServerVariable(bool withExtensionData = true)
    {
        return new ServerVariable()
        {
            Enum = new List<string> {"foo"},
            Default = "bar",
            Description = "baz",
            Examples = new List<string> {"quz"},
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    public static Channel Channel(bool withExtensionData = true)
    {
        return new Channel()
        {
            Description = "foo",
            Servers = new [] {"production"},
            Subscribe = new Operation(),
            Publish = new Operation(),
            Parameters = ImmutableDictionary<string, Parameter>.Empty,
            Bindings = ChannelBindings(),
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    public static Tag Tag(bool withExtensionData = true)
    {
        return new Tag()
        {
            Name = "foo",
            Description = "bar",
            ExternalDocs = new ExternalDocumentation(),
            Extensions = WithExtensionData(withExtensionData)
        };
    }

    public static ExternalDocumentation ExternalDocs(bool withExtensionData = true)
    {
        return new ExternalDocumentation()
        {
            Description = "foo",
            Url = new Uri("https://lego.com"),
            Extensions = WithExtensionData(withExtensionData)
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
            Extensions = WithExtensionData(withExtensionData)
        };
    }
}