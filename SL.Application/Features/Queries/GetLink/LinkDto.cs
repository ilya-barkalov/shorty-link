using AutoMapper;
using SL.Domain.Entities;

namespace SL.Application.Features.Queries.GetLink;

public class LinkDto
{
    public Guid Id { get; set; }
    
    public string OriginalUrl { get; set; }
    
    public string ShortUrl { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}

public class LinkDtoMapperProfile : Profile
{
    public LinkDtoMapperProfile()
    {
        CreateMap<Link, LinkDto>();
    }
}