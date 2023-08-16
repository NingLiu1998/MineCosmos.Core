using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MineCosmos.Core.Model;

namespace MineCosmos.Core.Api.Filter
{
    /// <summary>
    /// 统一返回格式
    /// 参照：https://learn.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0
    /// </summary>
    public class ApiResponseAttribute : TypeFilterAttribute
    {
        public ApiResponseAttribute() : base(typeof(ApiResponseFilter))
        {
        }

        private class ApiResponseFilter : IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext context)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    var wrappedResponse = new MessageModel<object>()
                    {
                        status = 200,
                        response = objectResult.Value,
                        msg = "success",
                        success = true
                    };

                    context.Result = new ObjectResult(wrappedResponse)
                    {
                        StatusCode = objectResult.StatusCode,
                        DeclaredType = objectResult.DeclaredType,
                        ContentTypes = objectResult.ContentTypes,
                        Formatters = objectResult.Formatters,
                        Value = wrappedResponse
                    };
                }
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
            }

        }
    }
}
