using Manage.Config.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Manage.Config.API.Utils
{
    public static class ResponseWrapperExtensions
    {
        public static ActionResult ToHttpResponse(this ResponseWrapper response)
        {
            if (response.IsNotFound)
            {
                return new NotFoundObjectResult(response);
            }

            if (response.IsClientError)
            {
                return new BadRequestObjectResult(response);
            }

            return new OkResult();
        }

        public static ActionResult ToHttpResponse<T>(this ResponseWrapper<T> response)
        {
            if (response.IsNotFound)
            {
                return new NotFoundObjectResult(response);
            }

            if (response.IsClientError)
            {
                return new BadRequestObjectResult(response);
            }

            return new OkObjectResult(response.Data);
        }
    }
}
