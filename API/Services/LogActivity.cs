using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Services
{
    public class LogActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();

            if (result.HttpContext.User.Identity is not { IsAuthenticated: true }) return;

            var userId = result.HttpContext.User.GetUserId();
            var userRepository = result.HttpContext.RequestServices.GetService<IUserRepository>();
            if (userRepository != null) 
                await userRepository.LogUserActive(userId);
        }
    }
}
