using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
   
    public abstract class ServerBinding<T> : BindingDeserializer, IBindingParser<IServerBinding> where T : IServerBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IServerBinding LoadBinding(PropertyNode parseNode) => LoadBinding("ServerBinding", parseNode, FixedFieldMap);
    }

    public abstract class ChannelBinding<T> : BindingDeserializer, IBindingParser<IChannelBinding> where T : IChannelBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IChannelBinding LoadBinding(PropertyNode node) => LoadBinding("ChannelBinding", node.Value, FixedFieldMap);
    }

    public abstract class OperationBinding<T> : BindingDeserializer, IBindingParser<IOperationBinding> where T : IOperationBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IOperationBinding LoadBinding(PropertyNode parseNode) => LoadBinding("OperationBinding", parseNode, FixedFieldMap);
    }

    public abstract class MessageBinding<T> : BindingDeserializer, IBindingParser<IMessageBinding> where T : IMessageBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IMessageBinding LoadBinding(PropertyNode parseNode) => LoadBinding("MessageBinding", parseNode, FixedFieldMap);
    }
}