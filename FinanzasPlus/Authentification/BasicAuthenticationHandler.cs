using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FinanzasPlus.Models;

namespace FinanzasPlus.Authentification;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    FinancesContext context
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
   
    {

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var cookie = Request.Cookies["Authorization"];
        if (cookie == null)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        if (string.IsNullOrEmpty(cookie))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        if (!cookie.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var token = cookie[6..];
        var credentialAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        var credentials = credentialAsString.Split(":");
        if (credentials.Length != 2)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var username = credentials[0];
        var password = credentials[1];

        var user = await context.Users
            .FirstOrDefaultAsync(target => username.Equals(target.Name) && password.Equals(target.Password));
        if (user == null)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, username) };

        var roles = await context.UserRoles
            .Where(rol => rol.Id == user.Id)
            .Include(rol => rol.Role)
            .ToListAsync();
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name!)));

        return AuthenticateResult.Success(new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic")),
            Scheme.Name
        ));
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            Response.Redirect($"Error/?statusCode={StatusCodes.Status403Forbidden}");
            return Task.CompletedTask;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Redirect("../Authentication/Login/");
            return Task.CompletedTask;
        }
    }

