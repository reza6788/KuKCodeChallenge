using Kuk.Services.Services.Note.Messaging;

namespace Kuk.Services.Services.Note.Implementation
{
    public interface INoteService
    {
        NoteGetAllPageResponse GetAllPaged(NoteGetAllPageRequest request);
        Task<NoteGetByIdResponse> GetByIdAsync(int id);
        Task<NoteCreateResponse> CreateAsync(NoteCreateRequest request);
        Task<NoteUpdateResponse> UpdateAsync(NoteUpdateRequest request);
        Task<NoteDeleteResponse> DeleteAsync(int id);
    }
}
