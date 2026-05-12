//using System.IdentityModel.Tokens.Jwt;

//namespace SocialMedia.API.Middlewares;
//public class TokenBlocklistMiddleware(RequestDelegate _next)
//{
//    public async Task InvokeAsync(HttpContext context, ITokenBlocklistService blocklist)
//    {
//        var token = context.Request.Headers["Authorization"]
//            .ToString()
//            .Replace("Bearer ", "");
//        if (token == "") {
//            await _next(context);
//            return;
//            }
//        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
//        var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
//        DateTime? expiry = (expClaim !=null)?DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime :null;

//        if (!string.IsNullOrEmpty(token) && blocklist.IsRevoked(token) || (expiry != null&& DateTime.UtcNow > expiry))
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsJsonAsync(new { message = "Token has been revoked" });
//            return;
//        }

//        await _next(context);
//    }
//}