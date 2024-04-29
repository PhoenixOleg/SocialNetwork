using SocialNetwork.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.ViewModels.Account
{
    public class ChatViewModel
    {
        public User You { get; set; }
        public User ToWhom { get; set; }

        public List<Message> MessagesHistory { get; set; }

        [Required]
        public MessageViewModel NewMessage { get; set; }

        public ChatViewModel() 
        {
            NewMessage = new MessageViewModel();
        }
    }
}
