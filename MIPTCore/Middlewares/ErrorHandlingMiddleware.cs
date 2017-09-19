using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UserManagment.Exceptions;

namespace MIPTCore.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private Task HandleExceptionAsync(HttpContext context, DomainException exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            
            
            if      (exception is DuplicateEmailException)                 code = HttpStatusCode.Conflict;
            else if (exception is ProfileNotProvidedException)             code = HttpStatusCode.BadRequest;
            else if (exception is ProfileShouldNotBeProvidedException)     code = HttpStatusCode.BadRequest;
            else return Task.CompletedTask;

            var result = JsonConvert.SerializeObject(new KeyValuePair<string, string>(exception.FieldName, exception.Message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }
}