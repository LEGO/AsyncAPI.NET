// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using LEGO.AsyncAPI.Attributes;

    public enum ExchangeType
    {
        [Display("default")]
        Default = 0,

        [Display("topic")]
        Topic,

        [Display("direct")]
        Direct,

        [Display("fanout")]
        Fanout,

        [Display("headers")]
        Headers,
    }
}