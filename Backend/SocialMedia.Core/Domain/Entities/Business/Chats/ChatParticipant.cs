using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Domain.Entities.Business.Chats;
public class ChatParticipant
{
    public Guid ChatId { get; set; }

    public Chat Chat { get; set; } = default!;

    public string UserId { get; set; } = default!;
}