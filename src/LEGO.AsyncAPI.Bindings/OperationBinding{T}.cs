using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
    public abstract class OperationBinding<T> : BindingDeserializer, IBindingParser<IOperationBinding>
        where T : IOperationBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IOperationBinding LoadBinding(PropertyNode parseNode) => LoadBinding("OperationBinding", parseNode, FixedFieldMap);
    }
}