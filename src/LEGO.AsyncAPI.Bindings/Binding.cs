namespace LEGO.AsyncAPI.Bindings
{
    using Models.Interfaces;
    using Readers.Interface;
    using Readers.ParseNodes;

    public abstract class Binding<T> : Binding, IBindingParser<T>
        where T : IBinding, new()
    {
        protected void ParseMap<T>(
            MapNode mapNode,
            T domainObject,
            FixedFieldMap<T> fixedFieldMap)
        {
            if (mapNode == null)
            {
                return;
            }

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, fixedFieldMap, null);
            }
        }
        
        public abstract T LoadBinding(PropertyNode node);
    }
}
