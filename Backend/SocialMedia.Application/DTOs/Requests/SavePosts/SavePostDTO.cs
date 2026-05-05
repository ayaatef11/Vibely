using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Domain.DTOs.Requests.SavePosts;
public class SavePostDTO
{
    public Guid UserId { set; get; }
    public Guid PostId { set; get; }
}
