using BCVP.Common.Helper;
using BCVP.EventBus.EventHandling;
using BCVP.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BCVP.EventBus
{
    public class BlogDeletedIntegrationEventHandler : IIntegrationEventHandler<BlogDeletedIntegrationEvent>
    {
        private readonly IBlogArticleServices _blogArticleServices;
        private readonly ILogger<BlogDeletedIntegrationEventHandler> _logger;

        public BlogDeletedIntegrationEventHandler(
            IBlogArticleServices blogArticleServices,
            ILogger<BlogDeletedIntegrationEventHandler> logger)
        {
            _blogArticleServices = blogArticleServices;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BlogDeletedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "BCVP", @event);

            ConsoleHelper.WriteSuccessLine($"----- Handling integration event: {@event.Id} at BCVP - ({@event})");

            await _blogArticleServices.DeleteById(@event.BlogId.ToString());
        }

    }
}
