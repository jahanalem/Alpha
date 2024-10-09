using Alpha.LoggerService;
using Alpha.Models.GlobalErrorHandling;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Alpha.Web.App.GlobalErrorHandling.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private string _requestId = string.Empty;
        private DateTime _errorTime;
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _requestId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
                _errorTime = DateTime.Now;
                _logger.LogError($"\n\n\n Something went wrong:\n >>> RequestId = {_requestId} \n >>> ErrorTime = {_errorTime} \n\n\n {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Redirect($"/Home/Error?requestId={_requestId}&timeOfError={_errorTime.ToString("yyyy-MM-dd_HH-mm-ss")}");
            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString());
        }
    }
}