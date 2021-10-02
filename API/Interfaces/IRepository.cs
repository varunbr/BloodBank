using System.Threading.Tasks;
using API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    interface IRepository
    {
        IPhotoService PhotoService { get; }
        DataContext DataContext { get; }
        IMapper Mapper { get; }
        Task<int> GetUserIdByUserName(string userName);
        Task<string> GetUserNameById(int id);
        Task<bool> UserExist(int id);
        Task<bool> UserExist(string userName);
        Task<bool> IsBankModerator(int bankId, int userId);
        Task<bool> IsBankAdmin(int bankId, int userId);
        Task<bool> IsAdmin(int userId);
        Task<string> UpdatePhoto(IFormFile file, int? userId, int? bankId);
        Task DeletePhoto(int? userId, int? bankId);
    }
}
