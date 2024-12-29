using System;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SA_Management.ViewModel;

namespace SA_Management.CustomMiddleware;

public class GlobalExceptionHandlingMiddleware
{
    public RequestDelegate _nextRequest { get; set; }

    public GlobalExceptionHandlingMiddleware(RequestDelegate nextRequest)
    {
        _nextRequest = nextRequest;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _nextRequest(httpContext);
        }
        catch(Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var error = new ErrorResponse(){
                StatusCode = StatusCodes.Status500InternalServerError,
                Message  = "Error: " + ex.Message
            };
            var seralizeError = JsonConvert.SerializeObject(error);
            await httpContext.Response.WriteAsync(seralizeError);
        }
    }
}
