using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SL.Application.Common.Exceptions;
using SL.Application.Features.Queries.GetLink;
using SL.WebAPI.Controllers;

namespace SL.WebAPI.UnitTests.Controllers;

public class LinkControllerTests
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsOkWithLinkDto()
    {
        // Arrange
        var linkDto = new LinkDto
        {
            Id = Guid.NewGuid(),
            OriginalUrl = "Url",
            ShortUrl = "Short url",
            CreatedDate = DateTimeOffset.UtcNow
        };
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(i => i.Send(It.IsAny<GetLinkQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => linkDto);
        
        var sut = new LinkController(mediatorMock.Object);

        // Act
        var action = await sut.Get(new GetLinkQuery(linkDto.Id), CancellationToken.None);

        // Assert
        action.Should().BeOfType<OkObjectResult>();
        
        var result = action.As<OkObjectResult>();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<LinkDto>();
                
        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetLinkQuery>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task Get_OnNotFoundFoundException_ReturnsNotFound()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var notFoundMessage = $"Link with Id {guid} was not found";
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(i => i.Send(It.IsAny<GetLinkQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException(notFoundMessage));
        
        var sut = new LinkController(mediatorMock.Object);

        // Act
        var action = await sut.Get(new GetLinkQuery(guid), CancellationToken.None);

        // Assert
        action.Should().BeOfType<NotFoundObjectResult>();
        
        var result = action.As<NotFoundObjectResult>();
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Value.As<string>().Should().Be(notFoundMessage);
        
        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetLinkQuery>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task Get_OnException_ReturnsStatusCode500()
    {
        // Arrange
        const string exceptionMessage = $"Some exception message";
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(i => i.Send(It.IsAny<GetLinkQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));
        
        var sut = new LinkController(mediatorMock.Object);

        // Act
        var action = await sut.Get(new GetLinkQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        action.Should().BeOfType<ObjectResult>();
        
        var result = action.As<ObjectResult>();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.Value.As<string>().Should().Be(exceptionMessage);
        
        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetLinkQuery>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}