// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;

    public class PatternFieldMap<T> : Dictionary<Func<string, bool>, Action<T, string, ParseNode>>
    {
    }
}