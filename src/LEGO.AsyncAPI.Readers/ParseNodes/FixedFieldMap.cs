// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;

    public class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
    }
}
