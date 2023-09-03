using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;
using SL.Application.Features.Commands.CreateLink;
using SL.Application.Services;
using SL.Domain.Entities;

namespace SL.Application.UnitTests.Features.Commands;

public class CreateLinkCommandHandlerTests
{
    private static ServiceProvider _serviceProvider;
    
    public CreateLinkCommandHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddInMemoryContext();
        services.AddTransient<RandomStringService>();
        services.AddTransient<IRequestHandler<CreateLinkCommand, Guid>, CreateLinkCommandHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_ShouldCreateLink_WhenLinkDoesNotExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var context = _serviceProvider.GetRequiredService<IApplicationDbContext>();
        
        // Act
        var guid = await mediator.Send(new CreateLinkCommand("url"));

        // Assert
        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == guid);
        link.Should().NotBeNull();
        link.OriginalUrl.Should().Be("url");
        link.CreatedDate.Date.Should().Be(DateTimeOffset.UtcNow.Date);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenLinkAlreadyExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var context = _serviceProvider.GetRequiredService<IApplicationDbContext>();

        await context.Links.AddAsync(new Link("url", "short url"));
        await context.SaveChangesAsync();
        
        // Act
        var action = () => mediator.Send(new CreateLinkCommand("url"));

        // Assert
        await action.Should().ThrowAsync<ValidationException>("Link with same url already exists");
    }
}