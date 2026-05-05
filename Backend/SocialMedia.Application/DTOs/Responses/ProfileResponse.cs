using SocialMedia.Application.DTOs.Responses.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Domain.DTOs.Responses;
public class ProfileResponse
{
    public Guid Id {  get; set; }
    public int PostCount { set; get; }
    public int FollowerCount { set; get; }
    public int FollowingCount { set; get; }
    public string? Bio { get; set; } = string.Empty;
    public string FullName { set; get; } = string.Empty;
    public string UserName { set; get; } = string.Empty;
    public string? Website { set; get; } = string.Empty;
    public string? Location { set; get; } = string.Empty;
    public Guid SocialMediaUserId { set; get; }

    public byte[]? ProfileImage { get; set; }
    public byte[]? BackgroundImage { get; set; }
    public string? ProfileImageContentType { get; set; }
    public string? BackgroundImageContentType { get; set; }
    public List<PostResponse> Posts { get; set; }
}