using System.Collections.Generic;

namespace AutoLibraryUI
{
    public class Roles
    {
        public const string Administrator = "Администратор";
        public const string Reader = "Читатель";
        public const string Librarian = "Библиотекарь";

        public static bool IsInRole(string role)
        {
            return System.Threading.Thread.CurrentPrincipal!.IsInRole(role);
        }

        public static bool IsLibrarian()
        {
            return Roles.IsInRole(Roles.Administrator) || Roles.IsInRole(Roles.Librarian);
        }

        public static bool IsAdministrator()
        {
            return Roles.IsInRole(Roles.Administrator);
        }

        public static List<string> GetRoles()
        {
            List<string> roles = new List<string>();

            if(Roles.IsInRole(Roles.Reader))
            {
                roles.Add(Roles.Reader);
            }

            if(Roles.IsInRole(Roles.Librarian))
            {
                roles.Add(Roles.Librarian);
                roles.Add(Roles.Reader);
            }

            if(Roles.IsInRole(Roles.Administrator))
            {
                roles.Add(Roles.Administrator);
                roles.Add(Roles.Librarian);
                roles.Add(Roles.Reader);
            }

            return roles;
        }
    }
}
