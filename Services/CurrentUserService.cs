using Stellaway.Common.Exceptions;

namespace Stellaway.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{

    public string ServerUrl => string.Concat(httpContextAccessor?.HttpContext?.Request.Scheme, "://", httpContextAccessor?.HttpContext?.Request.Host.ToUriComponent()) ?? throw new NotFoundException("not have url server");

}