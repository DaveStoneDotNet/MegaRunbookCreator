
namespace STAR.Originations.MegaRunbook.Contracts
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string UserDisplayName { get; set; }
        public bool HasRole { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsSuccessful { get; set; }
        public string PrimaryRoleName { get; set; }
        public string[] RoleNames { get; set; }
        public string UserInitials { get; set; }
    }
}
