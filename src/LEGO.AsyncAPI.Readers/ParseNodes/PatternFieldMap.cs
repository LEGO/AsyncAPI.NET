using System;
using System.Collections.Generic;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal class PatternFieldMap<T> : Dictionary<Func<string, bool>, Action<T, string, ParseNode>>
    {
    }
}