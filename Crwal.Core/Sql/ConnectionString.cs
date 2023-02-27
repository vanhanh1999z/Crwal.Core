using Crwal.Core.Base;
using System.Collections.Generic;

namespace Crwal.Core.Sql
{
    public class ConnectionString : IConnectionString
    {
        public string ServerName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DataBase { get; set; }
        public ConnectionString(string serverName, string user, string password)
        {
            this.ServerName = serverName;
            this.User = user;
            this.Password = password;
        }

        public ConnectionString(string serverName, string user, string password, string dataBase) : this(serverName, user, password)
        {
            DataBase = dataBase;
        }

        public string BuildConnectionString()
        {
            string dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = social_index_v2; User ID = {User}; Password = {dePass}";
        } public string BuildConnectionStringWithDb()
        {
            string dePass = StringCipher.Decrypt(Password);
            return $"Data Source={ServerName};Initial Catalog = {DataBase}; User ID = {User}; Password = {dePass}";
        }

    }
}
