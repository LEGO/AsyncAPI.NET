// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Pulsar
{
    using LEGO.AsyncAPI.Attributes;

    public enum Persistence
    {
        [Display("persistent")]
        Persistent,

        [Display("non-persistent")]
        NonPersistent,
    }
}
