using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TecWi_Web.Data.Dapper
{
    public class Dapper : IDapper
    {
        public Dapper()
        {

        }

        public void Dispose()
        {

        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            return await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType).ConfigureAwait(false);
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, string connectionString, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            IEnumerable<T> iEnumerable =  await db.QueryAsync<T>(sp, parms, commandType: commandType).ConfigureAwait(false);
            return iEnumerable.ToList();
        }
    }
}
