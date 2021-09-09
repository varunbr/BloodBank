using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper) { }


        public async Task<PagedList<MemberDto>> GetUsers(UserParams userParams)
        {
            var query = DataContext.Users.AsQueryable();

            query = query.BuildQuery(userParams);
            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageSize, userParams.PageNumber);
        }
    }
}
