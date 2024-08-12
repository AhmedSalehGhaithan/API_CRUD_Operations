using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace CRUD_Operations.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            if(!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"],out var authheader))
                return Task.FromResult(AuthenticateResult.Fail("Unknown Schema"));

            if(!authheader.Scheme.Equals("Basic",StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Unknown Schema"));

            var encodedCredentisls = authheader.Parameter;
            var decodedCredentisls = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentisls)); 
            var userNameAndPassword = decodedCredentisls.Split(':');

            if (userNameAndPassword[0] != "admin" || userNameAndPassword[1] != "password")
                return Task.FromResult(AuthenticateResult.Fail($"Invalid username or password"));

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, userNameAndPassword[0])
            }, "Basic");

            var principle = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principle,"Basic");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
