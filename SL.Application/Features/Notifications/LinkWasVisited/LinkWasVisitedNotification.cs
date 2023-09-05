using MediatR;
using SL.Application.Common.Interfaces;
using SL.Domain.Entities;

namespace SL.Application.Features.Notifications.LinkWasVisited;

public record LinkWasVisitedNotification(Guid LinkId) : INotification;

public class LinkWasVisitedNotificationHandler : INotificationHandler<LinkWasVisitedNotification>
{
    private readonly IApplicationDbContext _context;

    public LinkWasVisitedNotificationHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(LinkWasVisitedNotification wasVisitedNotification, CancellationToken cancellationToken)
    {
        await _context.LinkVisits.AddAsync(new LinkVisit(wasVisitedNotification.LinkId), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}