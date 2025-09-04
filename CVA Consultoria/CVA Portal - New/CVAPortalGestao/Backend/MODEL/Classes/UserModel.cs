using MODEL.Enumerators;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class UserModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.User; } }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
        public int AlterPassword { get; set; }
        public ProfileModel Profile { get; set; }
        public CollaboratorModel Collaborator { get; set; }
        public int FirstAccess { get; set; }
    }
}
