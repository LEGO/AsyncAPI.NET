namespace LEGO.AsyncAPI.Bindings
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public abstract class ChannelBinding<T> : Binding<T>
        where T : IChannelBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override T LoadBinding(PropertyNode node) => BindingDeserializer.LoadBinding("ChannelBinding", node.Value, this.FixedFieldMap);
    }
}