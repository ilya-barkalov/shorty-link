using MediatR;

namespace SL.Application.Features.Commands.CreateLink;

public record CreateLinkCommand(string Url) : IRequest<Guid>;