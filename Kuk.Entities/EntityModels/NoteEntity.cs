using Kuk.Entities.Common;

namespace Kuk.Entities.EntityModels
{
    public class NoteEntity : BaseEntityInt
    {
        public string Title { get; set; }
        public string TextBody { get; set; }
    }
}
