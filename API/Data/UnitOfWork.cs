using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public UnitOfWork(DataContext context, IMapper mapper, IPhotoService photoService, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
            _userManager = userManager;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper, _photoService);
        public IBankRepository BankRepository => new BankRepository(_context, _mapper, _photoService);
        public IRoleRepository RoleRepository => new RoleRepository(_context, _mapper, _photoService, _userManager);

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
