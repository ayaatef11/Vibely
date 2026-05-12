
namespace SocialMedia.Application.Helpers;
public class AppException(string message) : Exception(message);

// Specific ones
public class NotFoundException(string message) : AppException(message);
public class BadRequestException(string message) : AppException(message);
public class UnauthorizedException(string message) : AppException(message);