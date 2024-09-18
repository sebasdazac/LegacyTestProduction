using System.Security.Claims;
using System.Security.Principal;


namespace LegacyTest.Tools
{
    public class SessionHelper
    {

        public SessionHelper()
        {

        }
        public static string GetValue(IPrincipal User, string Property)
        {
            var r = ((ClaimsIdentity)User.Identity!).FindFirst(Property);
            return r == null ? "" : r.Value;
        }

        public static string GetNameIdentifier(IPrincipal User)
        {
            var r = ((ClaimsIdentity)User.Identity!).FindFirst(ClaimTypes.NameIdentifier);
            return r == null ? "" : r.Value;
        }

        public static string GetName(IPrincipal User)
        {
            var r = ((ClaimsIdentity)User.Identity!).FindFirst(ClaimTypes.Name);
            return r == null ? "" : r.Value;
        }
        public static string GetRol(IPrincipal User)
        {
            var r = ((ClaimsIdentity)User.Identity!).FindFirst(ClaimTypes.Role);
            return r == null ? "" : r.Value;
        }

        public static string GetIdRol(IPrincipal User)
        {
            var r = ((ClaimsIdentity)User.Identity!).FindFirst(ClaimTypes.Sid);
            return r == null ? "" : r.Value;
        }

        public static bool ValidarOperacion(IPrincipal User, int Op)
        {
            var r = ((ClaimsIdentity)User.Identity!).FindFirst("lstOperaciones");
            return false;
        }

      

    }
}
