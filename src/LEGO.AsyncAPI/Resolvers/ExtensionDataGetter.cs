// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Resolvers
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.NewtonUtils;

    internal class ExtensionDataGetter
    {
        public IEnumerable<KeyValuePair<object, object>> Getter(object o)
        {
            if (o is not IExtensible extensible)
            {
                return null;
            }

            if (extensible.Extensions == null)
            {
                return null;
            }

            IEnumerable<KeyValuePair<object, object>> obj = new Dictionary<object, object>();
            foreach (var (key, value) in extensible.Extensions)
            {
                obj = obj.Append(new KeyValuePair<object, object>(key, AnyToJToken.Map(value)));
            }

            return obj;
        }
    }
}