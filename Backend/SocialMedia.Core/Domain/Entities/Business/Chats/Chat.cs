using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Domain.Entities.Business.Chats;
public class Chat:BaseEntity<Guid>
{
    public bool IsGroup { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<ChatParticipant> Participants
        = new List<ChatParticipant>();

    public ICollection<Message> Messages
        = new List<Message>();
}