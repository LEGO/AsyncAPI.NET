namespace LEGO.AsyncAPI.Converters
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class ServerBindingConverter : BindingConverter<IServerBinding>
    {
        protected override ImmutableDictionary<string, Type> GetBindingTypeMap()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, Type>();
            builder.Add(new KeyValuePair<string, Type>("kafka", typeof(KafkaServerBinding)));
            return builder.ToImmutable();
        }
    }
}