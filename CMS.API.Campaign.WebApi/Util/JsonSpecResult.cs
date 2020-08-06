using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CMS.API.Campaign.WebApi.Util
{
    public class JsonSpecResult<T> : IActionResult
    {
        private readonly JsonSpecDto _result;
        private int? _statusCode;

        public int? StatusCode
        {
            get
            {
                return _statusCode;
            }
        }

        public JsonSpecDto Result
        {
            get
            {
                return _result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonDto">Throw Exception if content is null</param>
        public JsonSpecResult(JsonSpecDto jsonDto, int statusCode = (int)HttpStatusCode.OK)
        {
            _result = jsonDto ?? throw new ArgumentNullException(nameof(jsonDto));
            _statusCode = statusCode;
        }

        public JsonSpecResult(string errorMessage, int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            _result = new JsonSpecDto()
            {
                Error = new List<string> { errorMessage }
            };
            _statusCode = statusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult("")
            {
                StatusCode = _statusCode,
                Value = _result
            };

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}
