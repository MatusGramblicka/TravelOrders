using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Core;

public class CsvFileResult : FileResult
{
    private readonly Func<Stream, ActionContext, Task> _callback;

    public CsvFileResult(MediaTypeHeaderValue contentType, Func<Stream, ActionContext, Task> callback) :
        base(contentType?.ToString())
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        return _callback(context.HttpContext.Response.Body, context);
    }
}