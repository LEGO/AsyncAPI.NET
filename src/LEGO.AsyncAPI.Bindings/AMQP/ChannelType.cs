// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using LEGO.AsyncAPI.Attributes;

    public enum ChannelType
    {
        [Display("routingKey")]
        RoutingKey = 0,

        [Display("queue")]
        Queue,
    }
}