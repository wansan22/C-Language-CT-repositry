using System.Security.Claims;

namespace WebApi
{
    public static class ApiExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue("sub")!);
        }
    }
}
