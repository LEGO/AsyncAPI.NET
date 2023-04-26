namespace LEGO.AsyncAPI.Bindings
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public abstract class ChannelBinding<T> : BindingDeserializer, IBindingParser<IChannelBinding>
        where T : IChannelBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override IChannelBinding LoadBinding(PropertyNode node)
        {
            return this.LoadBinding("ChannelBinding", node.Value, this.FixedFieldMap);
        }
    }
}