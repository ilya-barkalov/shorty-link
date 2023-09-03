using MediatR;
using Microsoft.EntityFrameworkCore;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;

namespace SL.Application.Features.Commands.UpdateLink;

public class UpdateLinkCommandHandler : IRequestHandler<UpdateLinkCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLinkCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await _context.Links.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (link is null)
        {
            throw new NotFoundException($"Link with Id {request.Id} was not found");
        }

        link.OriginalUrl = request.OriginalUrl;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}