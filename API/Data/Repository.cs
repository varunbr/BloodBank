using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class Repository : IRepository
    {
        public DataContext DataContext { get; }
        public IMapper Mapper { get; }

        public Repository(DataContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            Mapper = mapper;
        }
    }
}
