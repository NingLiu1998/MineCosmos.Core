using MineCosmos.Core.Common;
using MineCosmos.Core.EventBus.EventHandling;
using MineCosmos.Core.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MineCosmos.Core.EventBus
{
    public class BlogQueryIntegrationEventHandler : IIntegrationEventHandler<BlogQueryIntegrationEvent>
    {
        private readonly IBlogArticleServices _blogArticleServices;
        private readonly ILogger<BlogQueryIntegrationEventHandler> _logger;

        public BlogQueryIntegrationEventHandler(
            IBlogArticleServices blogArticleServices,
            ILogger<BlogQueryIntegrationEventHandler> logger)
        {
            _blogArticleServices = blogArticleServices;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BlogQueryIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "MineCosmos.Core", @event);

            ConsoleHelper.WriteSuccessLine($"----- Handling integration event: {@event.Id} at MineCosmos.Core - ({@event})");

            await _blogArticleServices.QueryById(@event.BlogId.ToString());
        }

    }
}
