using Crwal.Core.Base;

namespace Crwal.Core.Sql
{
    public class ConnectionString : IConnectionString
    {
<<<<<<< HEAD
        public string ServerName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DataBase { get; set; }
=======
>>>>>>> 0fc86f9c04b153fd9cf6ddbb32e289c4765ba906
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
        } public string BuildConnectionStringWithDb()
        {
            string dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = {DataBase}; User ID = {User}; Password = {dePass}";
        }
    }
}