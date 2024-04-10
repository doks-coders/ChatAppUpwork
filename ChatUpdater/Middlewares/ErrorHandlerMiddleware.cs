using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models;
using System.Net;
using System.Text.Json;

namespace ChatUpdater.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = await ApiResponseModal<object>.FatalAsync(error, BaseErrorCodes.UnknownSystemException);

                switch (error)
                {
                    case ApiErrorException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel = await ApiResponseModal<object>.FatalAsync(e);
                        break;
                    /*
                    case NpgsqlException e:
                        responseModel =
                            await ApiResponseModal<object>.FatalAsync(e, BaseErrorCodes.DatabaseUnknownError);
                        break;
                    */

                    case KeyNotFoundException:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        // unhandled error
                        if (error.HResult == 401)
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        else
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
