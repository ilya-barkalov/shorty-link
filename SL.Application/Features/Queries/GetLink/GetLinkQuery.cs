using MediatR;

namespace SL.Application.Features.Queries.GetLink;

public record GetLinkQuery(Guid Id) : IRequest<LinkDto>;