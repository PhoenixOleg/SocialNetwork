using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models.Context;
using SocialNetwork.Models.Users;

namespace SocialNetwork.Data.Repository
{
    public class MessageRepository : Repository<Message>
    {
        public MessageRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<List<Message>> GetMessages(User sender, User recipient)
        {
            Set.Include(x => x.Recipient);
            Set.Include(x => x.Sender);

            var from = await Set.Where(x => x.SenderId == sender.Id && x.RecipientId == recipient.Id).ToListAsync();
            var to = await Set.Where(x => x.SenderId == recipient.Id && x.RecipientId == sender.Id).ToListAsync();

            var itog = new List<Message>();
            itog.AddRange(from);
            itog.AddRange(to);
            itog.OrderBy(x => x.Id);
            return itog;
        }
    }
}
