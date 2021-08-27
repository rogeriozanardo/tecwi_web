using System.Threading.Tasks;

namespace TecWi_Web.Data.UoW
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Rollback();
    }
}
