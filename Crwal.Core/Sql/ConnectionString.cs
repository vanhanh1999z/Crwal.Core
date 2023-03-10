using Crwal.Core.Base;

namespace Crwal.Core.Sql
{
    public class ConnectionString
    {
        public string ServerName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DataBase { get; set; }

        public ConnectionString(string serverName, string user, string password)
        {
            ServerName = serverName;
            User = user;
            Password = password;
        }

        public ConnectionString(string serverName, string user, string password, string dataBase) : this(serverName, user, password)
        {
            DataBase = dataBase;
        }

        public string BuildConnectionString()
        {
            var dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = social_index_v2; User ID = {User}; Password = {dePass}; CharSet=utf8; Pooling=True";
        }

        public string BuildConnectionStringWithDb()
        {
            string dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = {DataBase}; User ID = {User}; Password = {dePass}; CharSet=utf8; Pooling=True";
        }
    }
}