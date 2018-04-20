using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CapitalsTableHelper;
using Common.DomainSteroids;
using DonationManagment;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UserManagment.Exceptions;
using Loggly;

namespace MIPTCore.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private Task HandleExceptionAsync(HttpContext context, DomainException exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            
            
            if      (exception is DuplicateEmailException)                 code = HttpStatusCode.Conflict;
            else if (exception is EmailAlreadyConfirmedException)          code = HttpStatusCode.Conflict;
            else if (exception is OperationOnUserThatNotExistsException)   code = HttpStatusCode.BadRequest;            
            else if (exception is ProfileNotProvidedException)             code = HttpStatusCode.BadRequest;
            else if (exception is ProfileShouldNotBeProvidedException)     code = HttpStatusCode.BadRequest;    
            else if (exception is RelatedCapitalNotExists)                 code = HttpStatusCode.BadRequest;
            else if (exception is IvalidDonationTarget)                    code = HttpStatusCode.BadRequest;
            else if (exception is IvalidPaymentType)                       code = HttpStatusCode.BadRequest;
            else                                                           code = (HttpStatusCode) 500;

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
            catch(Exception e) when (LogToLoggly(e)) {}
        }

        private bool LogToLoggly(Exception e)
        {
            ILogglyClient loggly = new LogglyClient();
            var logEvent = new LogglyEvent();
            logEvent.Data.Add("description", "Non-Domain Exception", DateTime.Now);
            logEvent.Data.Add("exception", e);
            loggly.Log(logEvent);

            return false;
        }
    }
}