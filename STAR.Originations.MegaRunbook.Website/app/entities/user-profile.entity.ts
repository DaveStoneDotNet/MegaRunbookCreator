
export class UserProfile {
    HasRole: Boolean;
    IsAdministrator: Boolean;
    IsAuthenticated: Boolean;
    IsSuccessful: Boolean;
    PrimaryRoleName: string;
    RoleNames: string[];
    UserDisplayName: string;
    UserId: Number;
    UserInitials: string;
}

// {
//      "UserId":21981, 
//      "UserDisplayName":"Dave Stone", 
//      "HasRole":true, 
//      "IsAdministrator":true, 
//      "IsAuthenticated":false, 
//      "IsSuccessful":true, 
//      "PrimaryRoleName":"Administrator", 
//      "RoleNames":["Administrator"], 
//      "UserInitials":"LDS"
// }