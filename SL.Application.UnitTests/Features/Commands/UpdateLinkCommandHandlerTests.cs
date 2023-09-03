using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;
using SL.Application.Features.Commands.UpdateLink;
using SL.Domain.Entities;

namespace SL.Application.UnitTests.Features.Commands;

public class UpdateLinkCommandHandlerTests
{
    private static ServiceProvider _serviceProvider;
    
    public UpdateLinkCommandHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddInMemoryContext();
        services.AddTransient<IRequestHandler<UpdateLinkCommand>, UpdateLinkCommandHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_ShouldUpdateLink_WhenLinkExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var context = _serviceProvider.GetRequiredService<IApplicationDbContext>();

        var link = new Link("url", "short url");
        await context.Links.AddAsync(link);
        await context.SaveChangesAsync();
        
        // Act
        await mediator.Send(new UpdateLinkCommand(link.Id,"new url"));

        // Assert
        var linkAfterUpdate = await context.Links.FirstOrDefaultAsync(x => x.Id == link.Id);
        linkAfterUpdate.Should().NotBeNull();
        linkAfterUpdate.OriginalUrl.Should().Be("new url");
        linkAfterUpdate.ShortUrl.Should().Be(link.ShortUrl);
        linkAfterUpdate.CreatedDate.Should().Be(link.CreatedDate);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenLinkDoesNotExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var guid = Guid.NewGuid();
        
        // Act
        var action = () => mediator.Send(new UpdateLinkCommand(guid,"url"));

        // Assert
        await action.Should().ThrowAsync<NotFoundException>($"Link with Id {guid} was not found");
    }
}