using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
    public abstract class ServerBinding<T> : BindingDeserializer, IBindingParser<IServerBinding>
        where T : IServerBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IServerBinding LoadBinding(PropertyNode parseNode) => LoadBinding("ServerBinding", parseNode, FixedFieldMap);
    }
}