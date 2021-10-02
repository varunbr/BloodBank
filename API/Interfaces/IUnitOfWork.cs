using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IBankRepository BankRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task<bool> SaveChanges();
        bool HasChanges();
    }
}
