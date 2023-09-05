using AutoMapper;
using SL.Domain.Entities;

namespace SL.Application.Features.Queries.GetLinkByShortUrl;

public class LinkDto
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; }
}

public class LinkDtoMapperProfile : Profile
{
    public LinkDtoMapperProfile()
    {
        CreateMap<Link, LinkDto>();
    }
}