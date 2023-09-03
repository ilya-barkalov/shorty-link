using MediatR;

namespace SL.Application.Features.Commands.UpdateLink;

public record UpdateLinkCommand(Guid Id, string OriginalUrl) : IRequest;