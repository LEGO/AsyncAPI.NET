using System;
using System.Collections.Generic;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
    }
}