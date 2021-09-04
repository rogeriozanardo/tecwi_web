using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace TecWi_Web.Data.Dapper
{
    public interface IDapper : IDisposable
    {
        Task<T> Get<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
        Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text);
    }  
}
