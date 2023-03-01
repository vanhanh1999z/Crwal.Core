namespace Crwal.Core.Sql
{
    public interface IConnectionString
    {
        string DataBase { get; set; }
        string Password { get; set; }
        string ServerName { get; set; }
        string User { get; set; }

        string BuildConnectionString();
    }
}