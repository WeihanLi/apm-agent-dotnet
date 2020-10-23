using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CMS.API.Campaign.WebApi.Util
{
    public class JsonSpecResult<T> : IActionResult
    {
        public int StatusCode { get; }

        public JsonSpecDto Result { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="jsonDto">Throw Exception if content is null</param>
        /// <param name="statusCode">statusCode</param>
        public JsonSpecResult(JsonSpecDto jsonDto, int statusCode = (int)HttpStatusCode.OK)
        {
            Result = jsonDto ?? throw new ArgumentNullException(nameof(jsonDto));
            StatusCode = statusCode;
        }

        public JsonSpecResult(string errorMessage, int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            Result = new JsonSpecDto()
            {
                Error = new List<string> { errorMessage }
            };
            StatusCode = statusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult(Result)
            {
                StatusCode = StatusCode
            };

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}