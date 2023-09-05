using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;
using SL.Application.Features.Queries.GetLinkByShortUrl;
using SL.Domain.Entities;

namespace SL.Application.UnitTests.Features.Queries;

public class GetLinkByShortUrlQueryHandlerTests
{
    private static ServiceProvider _serviceProvider;
    
    public GetLinkByShortUrlQueryHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddAutoMapper(typeof(LinkDto).Assembly);

        services.AddInMemoryContext();
        services.AddTransient<IRequestHandler<GetLinkByShortUrlQuery, LinkDto>, GetLinkByShortUrlQueryHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnLinkDto_WhenLinkExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var context = _serviceProvider.GetRequiredService<IApplicationDbContext>();

        var link = new Link("url", "short url");
        await context.Links.AddAsync(link);
        await context.SaveChangesAsync();
        
        // Act
        var linkDto = await mediator.Send(new GetLinkByShortUrlQuery(link.ShortUrl));

        // Assert
        linkDto.Should().NotBeNull();
        linkDto.Id.Should().Be(link.Id);
        linkDto.OriginalUrl.Should().Be(link.OriginalUrl);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenLinkDoesNotExists()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        const string shortUrl = "short url";
        
        // Act
        var action = () => mediator.Send(new GetLinkByShortUrlQuery(shortUrl));

        // Assert
        await action.Should().ThrowAsync<NotFoundException>($"Link with ShortUrl {shortUrl} was not found");
    }
}