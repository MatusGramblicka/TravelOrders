using Interface.DatabaseAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelOrdersServer.ActionFilters;

public class ValidateTravelOrderExistsAttribute : IAsyncActionFilter
{
    private readonly IRepositoryManager _repository;

    public ValidateTravelOrderExistsAttribute(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var id = (int)(context.ActionArguments["id"] ?? throw new InvalidOperationException());
        var travelOrder = await _repository.TravelOrder.GetTravelOrderWithTrafficsAsync(id);

        if (travelOrder is null)
            context.Result = new NotFoundResult();
        else
        {
            context.HttpContext.Items.Add("travelOrder", travelOrder);
            await next();
        }
    }
}