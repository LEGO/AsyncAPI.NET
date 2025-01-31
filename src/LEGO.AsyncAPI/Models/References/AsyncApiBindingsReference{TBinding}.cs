// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiBindingsReference<TBinding> : AsyncApiBindings<TBinding>, IAsyncApiReferenceable
        where TBinding : IBinding
    {
        public bool UnresolvedReference { get => this.target == null; }

        public AsyncApiReference Reference { get; set; }

        private AsyncApiBindings<TBinding> target;

        private AsyncApiBindings<TBinding> Target
        {
            get
            {
                this.target ??= this.Reference.Workspace?.ResolveReference<AsyncApiBindings<TBinding>>(this.Reference);
                return this.target;
            }
        }

        public override void Add(TBinding binding)
        {
            this.Target.Add(binding);
        }

        public override ICollection<string> Keys => this.Target.Keys;

        public override ICollection<TBinding> Values => this.Target.Values;

        public override int Count => this.Target.Count;

        public override bool IsReadOnly => this.Target.IsReadOnly;

        public AsyncApiBindingsReference(string reference)
        {
            ReferenceType type = ReferenceType.None;
            if (typeof(TBinding) == typeof(IServerBinding))
            {
                type = ReferenceType.ServerBindings;
            }
            if (typeof(TBinding) == typeof(IMessageBinding))
            {
                type = ReferenceType.MessageBindings;
            }
            if (typeof(TBinding) == typeof(IOperationBinding))
            {
                type = ReferenceType.OperationBindings;
            }
            if (typeof(TBinding) == typeof(IChannelBinding))
            {
                type = ReferenceType.ChannelBindings;
            }

            if (type == ReferenceType.None)
            {
                throw new NotImplementedException($"Binding type '{typeof(TBinding)}' not supported.");
            }

            this.Reference = new AsyncApiReference(reference, type);
        }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV2(writer);
                return;
            }

            this.Target.SerializeV2(writer);
        }

        public override void Add(string key, TBinding value)
        {
            this.Target.Add(key, value);
        }

        public override bool ContainsKey(string key)
        {
            return this.Target.ContainsKey(key);
        }

        public override bool Remove(string key)
        {
            return this.Target.Remove(key);
        }

        public override bool TryGetValue(string key, out TBinding value)
        {
            return this.Target.TryGetValue(key, out value);
        }

        public override void Add(KeyValuePair<string, TBinding> item)
        {
            this.Target.Add(item);
        }

        public override void Clear()
        {
            this.Target.Clear();
        }

        public override bool Contains(KeyValuePair<string, TBinding> item)
        {
            return this.Target.Contains(item);
        }

        public override void CopyTo(KeyValuePair<string, TBinding>[] array, int arrayIndex)
        {
            this.Target.CopyTo(array, arrayIndex);
        }

        public override bool Remove(KeyValuePair<string, TBinding> item)
        {
            return this.Target.Remove(item);
        }

        public override IEnumerator<KeyValuePair<string, TBinding>> GetEnumerator()
        {
            return this.Target.GetEnumerator();
        }
    }
}
