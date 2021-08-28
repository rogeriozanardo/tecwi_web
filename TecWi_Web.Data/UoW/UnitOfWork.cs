using System.Threading.Tasks;
using TecWi_Web.Data.Context;

namespace TecWi_Web.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            
        }
    }
}
