using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
    public abstract class MessageBinding<T> : BindingDeserializer, IBindingParser<IMessageBinding>
        where T : IMessageBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IMessageBinding LoadBinding(PropertyNode parseNode) => LoadBinding("MessageBinding", parseNode, FixedFieldMap);
    }
}