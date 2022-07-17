using Kuk.Common.Enums;
using Kuk.Common.Messages;
using Kuk.Common.Utilities;
using Kuk.Data.IRepositories;
using Kuk.Entities.EntityModels;
using Kuk.Services.Common;
using Kuk.Services.Services.Note.Messaging;
using Kuk.Services.Services.Note.Validation;
using Kuk.Services.Services.Note.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Kuk.Services.Services.Note.Implementation
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public NoteGetAllPageResponse GetAllPaged(NoteGetAllPageRequest request)
        {
            try
            {
                var query = _noteRepository.TableNoTracking
                    .Where(p => !p.IsDeleted && (string.IsNullOrEmpty(request.Search) || p.Title.Contains(request.Search) || p.TextBody.Contains(request.Search))).AsQueryable();

                var noteEntities = query
                    .Select(s => new NoteGetAllPageResponseVm { Id = s.Id, Title = s.Title, TextBody = s.TextBody })
                    .ToList().Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

                return new NoteGetAllPageResponse
                {
                    Entity = noteEntities.ToList(),
                    RowCount = query.Count(),
                    IsSuccess = true,
                    Message = MessagesResource.GetSuccess,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new NoteGetAllPageResponse
                {
                    IsSuccess = false,
                    Message = MessagesResource.GetFailed,
                    Result = ResultType.Error,
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
                if (!validator.IsValid) return new NoteCreateResponse { Message = validator.Errors.GetErrors(), Result = ResultType.Warning };

                var existNote = await _noteRepository.TableNoTracking.AnyAsync(p => p.Title == request.Entity.Title);
                if (existNote) return new NoteCreateResponse { IsSuccess = false, Message = MessagesResource.DuplicateData, Result = ResultType.Warning };

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
                return new NoteCreateResponse
                {
                    IsSuccess = false,
                    Message = MessagesResource.AddFailed,
                    Result = ResultType.Error
                };
            }
        }

        public async Task<NoteUpdateResponse> UpdateAsync(NoteUpdateRequest request)
        {
            try
            {
                var validator = await new UpdateNoteValidator().ValidateAsync(request.Entity);
                if (!validator.IsValid) return new NoteUpdateResponse { Message = validator.Errors.GetErrors(), Result = ResultType.Warning };

                var existNote = await _noteRepository.TableNoTracking.AnyAsync(p => p.Id != request.Entity.Id && p.Title == request.Entity.Title);
                if (existNote) { return new NoteUpdateResponse { IsSuccess = false, Message = MessagesResource.DuplicateData, Result = ResultType.Warning }; }

                var noteEntity = await _noteRepository.GetByIdAsync(request.Entity.Id);
                if (noteEntity == null) return new NoteUpdateResponse { Message = MessagesResource.NotExistData, Result = ResultType.Warning };

                noteEntity.Title = request.Entity.Title;
                noteEntity.TextBody = request.Entity.TextBody;
                noteEntity.LastChangeDateTime = DateTime.Now;

                await _noteRepository.UpdateAsync(noteEntity, true, noteEntity.Id);

                return new NoteUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessagesResource.EditSuccess,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new NoteUpdateResponse
                {
                    IsSuccess = false,
                    Message = MessagesResource.EditFailed,
                    Result = ResultType.Error
                };
            }
        }

        public async Task<NoteDeleteResponse> DeleteAsync(int id)
        {
            try
            {
                var validator = await new GetByIdValidator().ValidateAsync(id);
                if (!validator.IsValid) return new NoteDeleteResponse { IsSuccess = false, Message = validator.Errors.GetErrors(), Result = ResultType.Warning };

                var noteEntity = await _noteRepository.GetByIdAsync(id);
                if (noteEntity == null) return new NoteDeleteResponse { IsSuccess = false, Message = MessagesResource.NotExistData, Result = ResultType.Warning };

                noteEntity.IsDeleted = true;
                noteEntity.DeleteDateTime = DateTime.Now;

                await _noteRepository.UpdateAsync(noteEntity, true, noteEntity.Id);

                return new NoteDeleteResponse { IsSuccess = true, Message = MessagesResource.DeleteSuccess, Result = ResultType.Success };
            }
            catch (Exception e)
            {
                return new NoteDeleteResponse { IsSuccess = false, Message = MessagesResource.DeleteFailed, Result = ResultType.Error };
            }
        }
    }
}
