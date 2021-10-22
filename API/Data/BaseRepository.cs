using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BaseRepository : IRepository
    {
        public IPhotoService PhotoService { get; }
        public DataContext DataContext { get; }
        public IMapper Mapper { get; }

        public BaseRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService)
        {
            PhotoService = photoService;
            DataContext = dataContext;
            Mapper = mapper;
        }

        #region User

        public async Task<int> GetUserIdByUserName(string userName)
        {
            return await DataContext.Users.Where(u => u.UserName == userName.ToLower())
                .Select(u => u.Id)
                .SingleAsync();
        }

        public async Task<string> GetUserNameById(int id)
        {
            return await DataContext.Users.Where(u => u.Id == id)
                .Select(u => u.UserName)
                .SingleAsync();
        }

        public async Task<bool> UserExist(int id)
        {
            return await DataContext.Users.AnyAsync(u => u.Id == id);
        }
        public async Task<bool> UserExist(string userName)
        {
            return await DataContext.Users.AnyAsync(u => u.UserName == userName.ToLower());
        }
        #endregion

        #region Bank
        public async Task<bool> IsBankExist(int bankId)
        {
            return await DataContext.Banks.AnyAsync(b => b.Id == bankId);
        }
        #endregion

        #region Moderator
        public async Task<bool> IsBankModerator(int bankId, int userId)
        {
            return await DataContext.Moderator.AnyAsync(m => m.UserId == userId && m.BankId == bankId);
        }

        public async Task<bool> IsBankAdmin(int bankId, int userId)
        {
            return await DataContext.Moderator.AnyAsync(m => m.UserId == userId &&
                                                             m.BankId == bankId && m.Type == "BankAdmin");
        }

        public async Task<bool> IsAdmin(int userId)
        {
            return await DataContext.UserRoles.AnyAsync(r => r.UserId == userId && r.Role.Name == "Admin");
        }
        #endregion

        #region Photo
        public async Task<string> UpdatePhoto(IFormFile file, int? userId, int? bankId)
        {
            var photo = await DataContext.Photos.SingleOrDefaultAsync(p => p.UserId == userId && p.BankId == bankId);
            var result = await PhotoService.UploadImage(file);
            if (result.Error != null) return null;
            await PhotoService.DeleteImage(photo.PublicId);
            photo.PublicId = result.PublicId;
            photo.Url = result.SecureUrl.AbsoluteUri;
            return photo.Url;
        }

        public async Task DeletePhoto(int? userId, int? bankId)
        {
            var photo = await DataContext.Photos.SingleOrDefaultAsync(p => p.UserId == userId && p.BankId == bankId);
            await PhotoService.DeleteImage(photo.PublicId);
            photo.PublicId = null;
            photo.Url = null;
        }
        #endregion
    }
}
