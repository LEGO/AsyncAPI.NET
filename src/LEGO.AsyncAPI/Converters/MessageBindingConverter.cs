namespace LEGO.AsyncAPI.Converters
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class MessageBindingConverter : BindingConverter<IMessageBinding>
    {
        protected override ImmutableDictionary<string, Type> GetBindingTypeMap()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, Type>();
            builder.Add(new KeyValuePair<string, Type>("http", typeof(HttpMessageBinding)));
            return builder.ToImmutable();
        }
    }
}