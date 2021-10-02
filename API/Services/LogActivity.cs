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
            if (context.HttpContext.User.Identity is { IsAuthenticated: true })
            {
                var userId = context.HttpContext.User.GetUserId();
                var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
                if (userRepository != null)
                    await userRepository.LogUserActive(userId);
            }

            var result = await next();
        }
    }
}
