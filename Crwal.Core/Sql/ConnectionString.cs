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

        public ConnectionString(string serverName, string user, string password, string database) : this(serverName, user, password)
        {
            Database = database;
        }

        private string ServerName { get; set; }
        private string User { get; set; }
        private string Password { get; set; }
        private string Database { get; set; }

        public string BuildConnectionString()
        {
            var dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = social_index_v2; User ID = {User}; Password = {dePass}";
        }
        public string BuildConnectionStringWithDb()
        {
            var dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = {Database}; User ID = {User}; Password = {dePass}";
        }
    }
}