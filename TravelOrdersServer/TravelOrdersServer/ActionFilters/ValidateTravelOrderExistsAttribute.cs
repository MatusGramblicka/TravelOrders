using Interface.DatabaseAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelOrdersServer.ActionFilters;

public class ValidateTravelOrderExistsAttribute(IRepositoryManager repository) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var id = (int)(context.ActionArguments["id"] ?? throw new InvalidOperationException());
        var travelOrder = await repository.TravelOrder.GetTravelOrderWithTrafficsAsync(id);

        if (travelOrder is null)
            context.Result = new NotFoundResult();
        else
        {
            context.HttpContext.Items.Add("travelOrder", travelOrder);
            await next();
        }
    }
}