using SocialMedia.Infrastructure.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Domain.DTOs.Responses;
public class FollowResponse
{
    public FollowState FollowState { set; get; }

    public Guid? FollowerId { set; get; }
    public Guid? FollowingId { set; get; }
}

