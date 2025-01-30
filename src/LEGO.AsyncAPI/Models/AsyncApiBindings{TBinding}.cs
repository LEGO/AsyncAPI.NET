// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiBindings<TBinding> : IDictionary<string, TBinding>, IAsyncApiSerializable
        where TBinding : IBinding
    {
        private Dictionary<string, TBinding> inner = new Dictionary<string, TBinding>();

        public virtual void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            foreach (var binding in this)
            {
                var bindingType = binding.Key;
                var bindingValue = binding.Value;

                writer.WritePropertyName(bindingType);

                bindingValue.SerializeV2(writer);
            }

            writer.WriteEndObject();
        }

        public virtual void Add(TBinding binding)
        {
            this[binding.BindingKey] = binding;
        }

        public virtual TBinding this[string key]
        {
            get => inner[key];
            set => inner[key] = value;
        }

        public virtual ICollection<string> Keys => inner.Keys;

        public virtual ICollection<TBinding> Values => inner.Values;

        public virtual int Count => inner.Count;

        public virtual bool IsReadOnly => ((IDictionary<string, TBinding>)inner).IsReadOnly;

        public virtual void Add(string key, TBinding value)
        {
            inner.Add(key, value);
        }

        public virtual bool ContainsKey(string key)
        {
            return inner.ContainsKey(key);
        }

        public virtual bool Remove(string key)
        {
            return inner.Remove(key);
        }

        public virtual bool TryGetValue(string key, out TBinding value)
        {
            return inner.TryGetValue(key, out value);
        }

        public virtual void Add(KeyValuePair<string, TBinding> item)
        {
            ((IDictionary<string, TBinding>)inner).Add(item);
        }

        public virtual void Clear()
        {
            inner.Clear();
        }

        public virtual bool Contains(KeyValuePair<string, TBinding> item)
        {
            return ((IDictionary<string, TBinding>)inner).Contains(item);
        }

        public virtual void CopyTo(KeyValuePair<string, TBinding>[] array, int arrayIndex)
        {
            ((IDictionary<string, TBinding>)inner).CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(KeyValuePair<string, TBinding> item)
        {
            return ((IDictionary<string, TBinding>)inner).Remove(item);
        }

        public virtual IEnumerator<KeyValuePair<string, TBinding>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }
    }
}
