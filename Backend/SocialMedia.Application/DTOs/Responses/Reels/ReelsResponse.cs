using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.DTOs.Responses.Reels;
public class ReelsResponse
{
    public string? FileName { get; set; }
    public DateTime UploadAt { get; set; }
    public string? ContentType { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();

    public Guid? UserId { get; set; }
}
