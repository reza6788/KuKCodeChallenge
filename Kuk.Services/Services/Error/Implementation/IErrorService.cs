using Kuk.Services.Services.Error.Messaging;

namespace Kuk.Services.Services.Error.Implementation
{
    public interface IErrorService
    {
        void AddError(AddErrorRequest request);
    }
}
