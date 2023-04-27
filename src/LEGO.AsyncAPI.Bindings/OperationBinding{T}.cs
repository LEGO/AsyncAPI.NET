using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
    public abstract class OperationBinding<T> : Binding<T>
        where T : IOperationBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override T LoadBinding(PropertyNode parseNode) => BindingDeserializer.LoadBinding("OperationBinding", parseNode, FixedFieldMap);
    }
}