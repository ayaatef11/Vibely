using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Domain.DTOs.Requests.Followers
{
    public class FollowDTO
    {
        public Guid Sender { set; get; }
        public Guid Reciever { set;get; }   
    }
}
