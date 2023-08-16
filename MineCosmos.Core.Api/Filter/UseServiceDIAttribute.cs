using MineCosmos.Core.IServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MineCosmos.Core.Filter
{
    public class UseServiceDIAttribute : ActionFilterAttribute
    {

        protected readonly ILogger<UseServiceDIAttribute> _logger;
        private readonly IBlogArticleServices _blogArticleServices;
        private readonly string _name;

        public UseServiceDIAttribute(ILogger<UseServiceDIAttribute> logger, IBlogArticleServices blogArticleServices,string Name="")
        {
            _logger = logger;
            _blogArticleServices = blogArticleServices;
            _name = Name;
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //var dd =await _blogArticleServices.Query();
            base.OnActionExecuted(context);
            DeleteSubscriptionFiles();
        }

        private void DeleteSubscriptionFiles()
        {
           
        }
    }
}
