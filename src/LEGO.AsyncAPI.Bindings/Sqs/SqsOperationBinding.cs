using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Sqs
{
    public class SqsOperationBinding : OperationBinding<SqsOperationBinding>
    {
        public override string BindingKey => "sqs";
        
        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            throw new System.NotImplementedException();
        }

        protected override FixedFieldMap<SqsOperationBinding> FixedFieldMap { get; }
    }
}