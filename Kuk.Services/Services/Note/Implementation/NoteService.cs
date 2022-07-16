using Kuk.Common.Enums;
using Kuk.Common.Messages;
using Kuk.Common.Utilities;
using Kuk.Data.IRepositories;
using Kuk.Entities.EntityModels;
using Kuk.Services.Common;
using Kuk.Services.Services.Error.Implementation;
using Kuk.Services.Services.Error.Messaging;
using Kuk.Services.Services.Error.ViewModel;
using Kuk.Services.Services.Note.Messaging;
using Kuk.Services.Services.Note.Validation;
using Kuk.Services.Services.Note.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Kuk.Services.Services.Note.Implementation
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IErrorService _errorService;

        public NoteService(INoteRepository noteRepository, IErrorService errorService)
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
                var noteEntities = query.ToList().Skip(request.PageSize).Take(request.Page);

                foreach (var noteEntity in noteEntities)
                {
                    noteGetAllPageResponseVms.Add(new NoteGetAllPageResponseVm
                    {
                        Id = noteEntity.Id,
                        Title = noteEntity.Title,
                        Summary = noteEntity.TextBody.Trim().Substring(0, 20),
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

        public async Task<NoteGetByIdResponse> GetByIdAsync(int id)
        {
            try
            {
                var validator = await new GetByIdValidator().ValidateAsync(id);
                if (!validator.IsValid) return new NoteGetByIdResponse { IsSuccess = false, Message = validator.Errors.GetErrors(), Result = ResultType.Warning };

                var noteEntity = await _noteRepository.TableNoTracking.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (noteEntity == null)
                    return new NoteGetByIdResponse { IsSuccess = false, Message = MessagesResource.NotExistData, Result = ResultType.Error };

                return new NoteGetByIdResponse
                {
                    Entity = new NoteGetByIdResponseVm { Id = noteEntity.Id, Title = noteEntity.Title, TextBody = noteEntity.TextBody },
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
                        ActionName = nameof(GetByIdAsync),
                        Table = nameof(TableType.Note),
                        Exception = e
                    }
                });
                return new NoteGetByIdResponse
                {
                    IsSuccess = false,
                    Message = MessagesResource.GetFailed,
                    Result = ResultType.Error
                };
            }
        }

        public async Task<NoteCreateResponse> CreateAsync(NoteCreateRequest request)
        {
            try
            {
                var validator = await new AddNoteValidator().ValidateAsync(request.Entity);
                if (!validator.IsValid)
                    return new NoteCreateResponse
                    {
                        Message = validator.Errors.GetErrors(),
                        Result = ResultType.Warning
                    };

                var existUser = await _noteRepository.TableNoTracking.AnyAsync(p => p.Title == request.Entity.Title);
                if (existUser) return new NoteCreateResponse { IsSuccess = false, Message = MessagesResource.DuplicateData, Result = ResultType.Warning };

                var noteEntity = new NoteEntity { Title = request.Entity.Title, TextBody = request.Entity.TextBody, CreateDateTime = DateTime.Now, };

                await _noteRepository.AddAsync(noteEntity);

                return new NoteCreateResponse
                {
                    IsSuccess = true,
                    Message = MessagesResource.AddSuccess,
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
                        ActionName = nameof(CreateAsync),
                        Table = nameof(TableType.Note),
                        Exception = e
                    }
                });
                return new NoteCreateResponse
                {
                    IsSuccess = false,
                    Message = MessagesResource.AddFailed,
                    Result = ResultType.Error
                };
            }
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
