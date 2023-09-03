using MediatR;

namespace SL.Application.Features.Commands.DeleteLink;

public record DeleteLinkCommand(Guid Id) : IRequest;