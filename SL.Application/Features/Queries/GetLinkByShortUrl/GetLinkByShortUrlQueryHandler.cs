using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;

namespace SL.Application.Features.Queries.GetLinkByShortUrl;

public class GetLinkByShortUrlQueryHandler : IRequestHandler<GetLinkByShortUrlQuery, LinkDto>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    
    public GetLinkByShortUrlQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<LinkDto> Handle(GetLinkByShortUrlQuery request, CancellationToken cancellationToken)
    {
        var linkDto = await _context.Links
            .Where(x => x.ShortUrl == request.ShortUrl)
            .ProjectTo<LinkDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (linkDto is null)
        {
            throw new NotFoundException($"Link with ShortUrl {request.ShortUrl} was not found");
        }

        return linkDto;
    }
}