using API.Data;
using AutoMapper;

namespace API.Interfaces
{
    interface IRepository
    {
        DataContext DataContext { get; }
        IMapper Mapper { get; }
    }
}
