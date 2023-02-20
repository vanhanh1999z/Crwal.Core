using Crwal.Core.Base;

namespace Crwal.Core.Sql
{
    public class ConnectionString
    {
        public string ServerName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public ConnectionString(string serverName, string user, string password)
        {
            this.ServerName = serverName;
            this.User = user;
            this.Password = password;
        }

        public string BuildConnectionString()
        {
            string dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = social_index_v2; User ID = {User}; Password = {dePass}";
        }
    }
}
