// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings
{
    using LEGO.AsyncAPI.Attributes;

    public enum BindingType
    {
        [Display("kafka")]
        Kafka,

        [Display("http")]
        Http,

        [Display("pulsar")]
        Pulsar,
    }
}
