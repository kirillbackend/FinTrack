using System.ComponentModel;

namespace FinTrack.Enums
{
    public enum UserRole
    {
        [Description("User")]
        User = 0,
        [Description("Administrator")]
        Administrator = 1
    }
}
