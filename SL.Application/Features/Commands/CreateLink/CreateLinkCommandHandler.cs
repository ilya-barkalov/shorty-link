using MediatR;
using Microsoft.EntityFrameworkCore;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;
using SL.Application.Services;
using SL.Domain.Entities;

namespace SL.Application.Features.Commands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly RandomStringService _randomStringService; 

    public CreateLinkCommandHandler(IApplicationDbContext context, RandomStringService randomStringService)
    {
        _context = context;
        _randomStringService = randomStringService;
    }

    public async Task<Guid> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Links.AnyAsync(x => x.OriginalUrl == request.Url, cancellationToken))
        {
            throw new ValidationException("Link with same url already exists");
        }

        var shortUrl = _randomStringService.GetRandomString();
        while (await _context.Links.AnyAsync(x => x.ShortUrl == shortUrl, cancellationToken))
        {
            shortUrl = _randomStringService.GetRandomString();
        }
        
        var link = new Link(request.Url, shortUrl);
        
        await _context.Links.AddAsync(link, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return link.Id;
    }
}