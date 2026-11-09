using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace raceday_mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequireRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;
    public RequireRoleAttribute(params string[] roles) => _roles = roles;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session;
        if (session.GetInt32("UserId") is null || string.IsNullOrWhiteSpace(session.GetString("Role")))
        {
            context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
            return;
        }
        if (_roles.Length > 0 && !_roles.Contains(session.GetString("Role"), StringComparer.OrdinalIgnoreCase))
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
    }
}
