using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SL.Application.Common.Exceptions;
using SL.Application.Common.Interfaces;

namespace SL.Application.Features.Queries.GetLink;

public class GetLinkQueryHandler : IRequestHandler<GetLinkQuery, LinkDto>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetLinkQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<LinkDto> Handle(GetLinkQuery request, CancellationToken cancellationToken)
    {
        var link = await _context.Links
            .ProjectTo<LinkDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (link is null)
        {
            throw new NotFoundException($"Link with Id {request.Id} was not found");
        }
        
        return link;
    }
}