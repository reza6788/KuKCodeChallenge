using Kuk.Common.Enums;
using Kuk.Common.Messages;
using Kuk.Data.IRepositories;
using Kuk.Services.Services.Error.Implementation;
using Kuk.Services.Services.Error.Messaging;
using Kuk.Services.Services.Error.ViewModel;
using Kuk.Services.Services.Note.Messaging;
using Kuk.Services.Services.Note.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Kuk.Services.Services.Note.Implementation
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IErrorService _errorService;

        public NoteService(INoteRepository noteRepository,IErrorService errorService)
        {
            _noteRepository = noteRepository;
            _errorService = errorService;
        }

        public NoteGetAllPageResponse GetAllPaged(NoteGetAllPageRequest request)
        {
            try
            {
                var noteGetAllPageResponseVms = new List<NoteGetAllPageResponseVm>();
                var query = _noteRepository.TableNoTracking
                    .Where(p => !p.IsDeleted && (string.IsNullOrEmpty(request.Search) ||
                                                 p.Title.Contains(request.Search) ||
                                                 p.TextBody.Contains(request.Search))).AsQueryable();
                var noteEntities=query.ToList().Skip(request.PageSize).Take(request.Page);

                foreach (var noteEntity in noteEntities)
                {
                    noteGetAllPageResponseVms.Add(new NoteGetAllPageResponseVm
                    {
                        Id = noteEntity.Id,
                        Title = noteEntity.Title,
                        Summary = noteEntity.TextBody.Trim().Substring(0,20),
                    });
                }

                return new NoteGetAllPageResponse
                {
                    Entity = noteGetAllPageResponseVms,
                    RowCount = query.Count(),
                    IsSuccess = true,
                    Message = MessagesResource.GetSuccess,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                _errorService.AddError(new AddErrorRequest
                {
                    Entity = new AddErrorVm
                    {
                        ServiceName = nameof(NoteService),
                        ActionName = nameof(GetAllPaged),
                        Table = nameof(TableType.Note),
                        Exception = e
                    }
                });
                return new NoteGetAllPageResponse
                {
                    IsSuccess = false,
                    Message = MessagesResource.GetFailed,
                    Result = ResultType.Error
                };
            }
        }

        public Task<NoteGetByIdResponse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NoteCreateResponse> CreateAsync(NoteCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<NoteUpdateResponse> UpdateAsync(NoteUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<NoteDeleteResponse> DeleteAsync(NoteDeleteRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
