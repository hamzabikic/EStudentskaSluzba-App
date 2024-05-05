using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EStudentskaSluzba.Services
{
    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute():base(typeof(AuthFilter))
        {

        }
    }

    public class AuthFilter : IAsyncActionFilter
    {
        private readonly AuthService service;

        public AuthFilter(AuthService _service)
        {
            service = _service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var info = await service.getInfo();
            if(info.JeLogiran)
            {
                await next();
            }
            else
            {
                throw new UnauthorizedAccessException("Nemate pravo pristupa!");
            }
        }
    }
}
