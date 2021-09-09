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
    public class BankRepository : Repository, IBankRepository
    {
        public BankRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<PagedList<BankDto>> GetUsers(BankParams bankParams)
        {
            var query = DataContext.Banks.AsQueryable();

            query = query.BuildQuery(bankParams);
            return await PagedList<BankDto>.CreateAsync(
                query.ProjectTo<BankDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                bankParams.PageSize, bankParams.PageNumber);
        }
    }
}
