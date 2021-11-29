using Services.ServiceReference;

namespace Passpot.Business
{
    public interface IControlValueChanged
    {
        bool HasChanges { get; }
        string NewValue { get; }
        FieldMetaDataItem MetaData { get; }
    }
}
