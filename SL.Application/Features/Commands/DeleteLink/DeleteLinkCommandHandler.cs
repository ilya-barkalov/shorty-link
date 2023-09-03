using MediatR;
using Microsoft.EntityFrameworkCore;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;

namespace SL.Application.Features.Commands.DeleteLink;

public class DeleteLinkCommandHandler : IRequestHandler<DeleteLinkCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLinkCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await _context.Links.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (link is null)
        {
            throw new NotFoundException($"Link with Id {request.Id} was not found");
        }

        _context.Links.Remove(link);
        await _context.SaveChangesAsync(cancellationToken);
    }
}