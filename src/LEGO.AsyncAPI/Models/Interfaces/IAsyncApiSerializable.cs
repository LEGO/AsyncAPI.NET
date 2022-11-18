namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Writers;

    public interface IAsyncApiSerializable : IAsyncApiElement
    {
        void SerializeV2(IAsyncApiWriter writer);
    }
}
