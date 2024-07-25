using Newtonsoft.Json.Linq;

namespace EventManagementApp.Exceptions
{
    public class AuthenticatenFailed : Exception
    {
        public AuthenticatenFailed(): base("Invalid Google token") { }
    }
}
