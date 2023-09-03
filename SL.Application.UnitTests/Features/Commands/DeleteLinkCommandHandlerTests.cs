using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;
using SL.Application.Features.Commands.DeleteLink;
using SL.Domain.Entities;

namespace SL.Application.UnitTests.Features.Commands;

public class DeleteLinkCommandHandlerTests
{
    private static ServiceProvider _serviceProvider;
    
    public DeleteLinkCommandHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddInMemoryContext();
        services.AddTransient<IRequestHandler<DeleteLinkCommand>, DeleteLinkCommandHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_ShouldDeleteLink_WhenLinkExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var context = _serviceProvider.GetRequiredService<IApplicationDbContext>();

        var link = new Link("url", "short url");
        await context.Links.AddAsync(link);
        await context.SaveChangesAsync();
        
        // Act
        await mediator.Send(new DeleteLinkCommand(link.Id));

        // Assert
        var linkExists = await context.Links.AnyAsync(x => x.Id == link.Id);
        linkExists.Should().Be(false);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenLinkDoesNotExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var guid = Guid.NewGuid();
        
        // Act
        var action = () => mediator.Send(new DeleteLinkCommand(guid));

        // Assert
        await action.Should().ThrowAsync<NotFoundException>($"Link with Id {guid} was not found");
    }   
}