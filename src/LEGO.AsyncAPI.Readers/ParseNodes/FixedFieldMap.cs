using System;
using System.Collections.Generic;

namespace LEGO.AsyncApi.Readers.ParseNodes
{
    internal class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
    }
}