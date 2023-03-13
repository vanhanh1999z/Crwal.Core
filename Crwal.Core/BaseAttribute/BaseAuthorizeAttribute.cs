using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Crwal.Core.BaseAttribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class BaseAuthorizeAttribute : Attribute
    {
        private string _conn { get; set; }
        private string _role { get; set; }
        private string[] _roles { get; set; }
        public BaseAuthorizeAttribute(string conn, string role)
        {
            _conn = conn;
            _role = role;
        }
        public BaseAuthorizeAttribute(string conn, params string[] roles)
        {
            _conn = conn;
            _roles = roles;
        }
        public bool IsAuthorized()
        {

            if (_roles != null && _roles.Length > 0)
            {
                if (_roles.Contains("admin")) return true;
            }

            if (_role == "admin")
            {
                return true;

            }
            return false;
        }
    }
}
