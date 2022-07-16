namespace Kuk.Services.Services.Note.ViewModel
{
    public class NoteDeleteRequestVm
    {
        public int Id { get; set; }
        public int? DeletedBy { get; set; }
        public string DeleteReason { get; set; }
    }
}
