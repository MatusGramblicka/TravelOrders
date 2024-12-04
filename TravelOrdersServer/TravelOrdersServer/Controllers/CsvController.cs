using Core;
using Interface.Managers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Globalization;

namespace TravelOrdersServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CsvController(ICsvManager csvManager) : ControllerBase
{
    [HttpGet]
    [Route("file")]
    [Produces("application/octet-stream")]
    public IActionResult GetCsv()
    {
        // https://stackoverflow.com/questions/65849270/asp-net-core-allowsynchronousio-true-per-endpoint-vs-per-server
        var allowSynchronousIoOption = HttpContext.Features.Get<IHttpBodyControlFeature>();
        if (allowSynchronousIoOption is not null)
            allowSynchronousIoOption.AllowSynchronousIO = true;
        

        var fileDownloadName =
            $"travelOrders-{DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}.csv";
        var contentDisposition = new ContentDispositionHeaderValue("attachment");
        contentDisposition.SetHttpFileName(fileDownloadName);
        Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
        Response.ContentType = "application/octet-stream";

        return new CsvFileResult(new MediaTypeHeaderValue(Response.ContentType),
            async (outputStream, context) => await csvManager.GenerateCsv(outputStream));
    }
}