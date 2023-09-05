using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Common.Interfaces;
using SL.Application.Features.Notifications.LinkWasVisited;
using SL.Domain.Entities;

namespace SL.Application.UnitTests.Features.Notifications;

public class LinkWasVisitedNotificationHandlerTests
{
    private static ServiceProvider _serviceProvider;
    
    public LinkWasVisitedNotificationHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddInMemoryContext();
        services.AddTransient<INotificationHandler<LinkWasVisitedNotification>, LinkWasVisitedNotificationHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_ShouldCreateLinkVisit_WhenLinkExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var context = _serviceProvider.GetRequiredService<IApplicationDbContext>();

        var link = new Link("url", "short url");
        await context.Links.AddAsync(link);
        await context.SaveChangesAsync();
        
        // Act
        await mediator.Publish(new LinkWasVisitedNotification(link.Id));

        // Assert
        var linkVisit = await context.LinkVisits.FirstOrDefaultAsync(x => x.LinkId == link.Id);
        linkVisit.Should().NotBeNull();
        linkVisit.CreatedDate.Date.Should().Be(DateTimeOffset.UtcNow.Date);
    }
}