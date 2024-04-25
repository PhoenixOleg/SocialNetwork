using SocialNetwork.Models.Users;

namespace SocialNetwork.ViewModels.Account
{
    public class UserWithFriendExt : User
    {
        public bool IsFriendWithCurrent { get; set; }
    }
}
