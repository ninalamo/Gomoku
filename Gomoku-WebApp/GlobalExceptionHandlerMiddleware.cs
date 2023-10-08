using FluentValidation;
using Gomoku_WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gomoku_WebApp;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            _logger.LogError(validationException, validationException.Message);

            var response = new BadRequestObjectResult(ResponseDto.Failed(string.Join(Environment.NewLine,validationException.Errors)));
        
            await response.ExecuteResultAsync(new ActionContext
            {
                HttpContext = context
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            // Create an IActionResult response based on the exception
            // You can customize the response as needed
            var response = new ObjectResult(ResponseDto.Failed(ex.Message))
            {
                StatusCode = 500
            };
            
            await response.ExecuteResultAsync(new ActionContext
            {
                HttpContext = context
            });
        }
    }
}