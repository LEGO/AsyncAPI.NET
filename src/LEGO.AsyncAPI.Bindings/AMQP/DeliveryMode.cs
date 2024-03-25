// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.AMQP
{
    using LEGO.AsyncAPI.Attributes;

    public enum DeliveryMode
    {
        [Display("transient")]
        Transient = 1,

        [Display("persistent")]
        Persistent = 2,
    }
}