using Kuk.Services.Services.Error.Messaging;

namespace Kuk.Services.Services.Error.Implementation
{
    public class ErrorService : BaseService, IErrorService
    {
        private readonly IErrorRepository _errorRepository;
        public ErrorService(IErrorRepository errorRepository)
        {
            _errorRepository = errorRepository;
        }

        public void AddError(AddErrorRequest request)
        {
            try
            {

                if (AppSettings.IsErrorLogEnabled)

                    _errorRepository.Add(new ErrorLogEntity()
                    {
                        DateTime = DateTime.Now,
                        ControllerName = request.Entity.ControllerName ?? string.Empty,
                        ActionName = request.Entity.ActionName ?? string.Empty,
                        ServiceName = request.Entity.ServiceName ?? string.Empty,
                        Table = request.Entity.Table ?? string.Empty,
                        TableId = request.Entity.TableId ?? 0,
                        UserId = UserId,
                        Variables = request.Entity.Variables.ToJson() ?? string.Empty,
                        Ip = GetUserIp() ?? string.Empty,
                        Exception = request.Entity.Exception.ToString(),
                        // Exception = $"{request.Entity.Exception.Message} {request.Entity.Exception.InnerException?.Message} {request.Entity.Exception.InnerException?.InnerException?.Message}",
                        ExceptionTypeName = request.Entity.Exception.GetType().Name
                    });
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
