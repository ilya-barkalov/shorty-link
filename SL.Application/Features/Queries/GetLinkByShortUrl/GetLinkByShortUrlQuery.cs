using MediatR;

namespace SL.Application.Features.Queries.GetLinkByShortUrl;

public record GetLinkByShortUrlQuery(string ShortUrl) : IRequest<LinkDto>;