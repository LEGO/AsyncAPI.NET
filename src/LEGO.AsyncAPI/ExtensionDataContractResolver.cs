using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LEGO.AsyncAPI;
// https://stackoverflow.com/a/41094764

public class ExtensionDataContractResolver : DefaultContractResolver
{
    private readonly ExtensionDataSetter extensionDataSetter = new ();
    private readonly ExtensionDataGetter extensionDataGetter = new ();

    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var jsonProperties = base.CreateProperties(type, memberSerialization);
        try
        {
            var firstOrDefault = jsonProperties.First(property => property.PropertyName.Equals("Extensions"));
            firstOrDefault.Ignored = true;
        }
        catch (InvalidOperationException e)
        {
            // no extensions found. This will only happen when the Model object does not extend the IExtensible interface
        }

        return jsonProperties;
    }

    public override JsonContract ResolveContract(Type type)
    {
        var resolveContract = base.ResolveContract(type);
        if (resolveContract.GetType() == typeof(JsonObjectContract))
        {
            var jsonObjectContract = resolveContract as JsonObjectContract;
            jsonObjectContract.ExtensionDataSetter = extensionDataSetter.Setter;
            jsonObjectContract.ExtensionDataGetter = extensionDataGetter.Getter;
        }

        return resolveContract;
    }
}