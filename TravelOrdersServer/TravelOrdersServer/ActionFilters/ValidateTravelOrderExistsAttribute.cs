using Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

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
        var id = (int)context.ActionArguments["id"];
        var travelOrder = await _repository.TravelOrder.GetTravelOrderWithTrafficsAsync(id);

        if (travelOrder == null)
        {
            context.Result = new NotFoundResult();
        }
        else
        {
            context.HttpContext.Items.Add("travelOrder", travelOrder);
            await next();
        }
    }
}