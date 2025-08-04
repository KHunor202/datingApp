using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

public class UserActivity : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}
