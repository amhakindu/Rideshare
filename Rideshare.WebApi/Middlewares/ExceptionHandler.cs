using System.Net;
using MediatR;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Responses;
using Serilog;

namespace Rideshare.WebApi.Middlewares;

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class ExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try{
            await next(context);
        }
        catch (Exception ex){
            // Handle the exception and generate an appropriate response
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Log the exception
        Log.Error(exception, "An unhandled exception occurred: ");

        // Return a proper response to client
        if(exception is ValidationException){
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
                new BaseResponse<Unit>{
                    Success=false,
                    Message="Request Data Validation Failed",
                    Errors=new List<string>{exception.Message}
                }
            );
            return;
        }else if(exception is NotFoundException){
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
                new BaseResponse<Unit>{
                    Success=false,
                    Message="Resource Not Found",
                    Errors=new List<string>{exception.Message}
                }
            );
            return;
        }else if(exception is InternalServerErrorException){
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
                new BaseResponse<Unit>{
                    Success=false,
                    Message="Failed to Process Request",
                    Errors=new List<string>{exception.Message}
                }
            );
            return;
        }
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
            new BaseResponse<Unit>{
                Success=false,
                Message="Failed To Process Request",
                Errors=new List<string>{exception.Message}
            }
        );
    }
}