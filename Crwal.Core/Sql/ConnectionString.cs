using Crwal.Core.Base;

namespace Crwal.Core.Sql
{
    public class ConnectionString
    {
        public ConnectionString(string serverName, string user, string password)
        {
            ServerName = serverName;
            User = user;
            Password = password;
        }

        private string ServerName { get; set; }
        private string User { get; set; }
        private string Password { get; set; }

        public string BuildConnectionString()
        {
            var dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = social_index_v2; User ID = {User}; Password = {dePass}";
        }
    }
}