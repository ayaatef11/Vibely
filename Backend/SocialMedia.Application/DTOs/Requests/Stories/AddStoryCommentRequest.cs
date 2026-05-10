using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.DTOs.Requests.Stories;
    public class AddStoryCommentRequest
    {
        public string Text { get; set; }
        public Guid ProfileId { set; get; }
        public Guid StoryId { set; get; }
    }
