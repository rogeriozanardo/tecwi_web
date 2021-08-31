using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace TecWi_Web.Data.Dapper
{
    public interface IDapper : IDisposable
    {
        DbConnection GetDbconnection(string connection);
        T Get<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
        List<T> GetAll<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
        int Execute(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
        T Insert<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
        T Update<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
    }  
}
