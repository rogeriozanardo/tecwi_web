using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TecWi_Web.Data.Context
{
    public class DapperDataContext : DbContext
    {
        public DapperDataContext() { }
        public DapperDataContext(DbContextOptions<DapperDataContext> options) : base(options) { }
    }  
}
