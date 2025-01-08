using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Core;

public class CsvFileResult(MediaTypeHeaderValue contentType, Func<Stream, ActionContext, Task> callback)
    : FileResult(contentType?.ToString())
{
    private readonly Func<Stream, ActionContext, Task> _callback =
        callback ?? throw new ArgumentNullException(nameof(callback));

    public override Task ExecuteResultAsync(ActionContext context)
    {
        return _callback(context.HttpContext.Response.Body, context);
    }
}