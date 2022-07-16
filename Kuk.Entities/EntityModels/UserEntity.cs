using Kuk.Entities.Common;

namespace Kuk.Entities.EntityModels
{
    public class UserEntity : BaseEntityInt
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
