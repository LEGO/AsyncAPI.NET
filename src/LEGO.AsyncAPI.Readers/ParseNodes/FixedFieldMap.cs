namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;

    internal class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
    }
}