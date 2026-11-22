using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools.Utils;

public static class ConnectionStringUtil
{
    public static string GetConnectionString(ConnectionStringBase connectionStringBase)
    {
        return GetConnectionString(
            connectionStringBase.ServerName, 
            connectionStringBase.Port,
            connectionStringBase.DatabaseName,
            connectionStringBase.UserName, 
            connectionStringBase.Password
        );
    }
    
    public static string GetConnectionString(string serverName, string port, string databaseName, string userName, string password)
    {
        return $"Server={serverName};Port={port};Database={databaseName};User Id={userName};Password={password};TrustServerCertificate=true";
    }
}