using SocialMedia.Core.Domain.Entities.Business.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.DTOs.Responses.Stories;
    public class StoryCommentResponse
    {
        public string Text { set; get; }
        public long ReactCount { set; get; }
        public DateTime AddedAt { set; get; }

        public Guid StoryId { set; get; }
        public Guid ProfileId { set; get; } 
    }
