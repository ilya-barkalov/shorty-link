using MediatR;
using Microsoft.AspNetCore.Mvc;
using SL.Application.Common.Exceptions;
using SL.Application.Features.Commands.CreateLink;
using SL.Application.Features.Commands.DeleteLink;
using SL.Application.Features.Commands.UpdateLink;
using SL.Application.Features.Queries.GetLink;

namespace SL.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LinkController : ControllerBase
{
    private readonly IMediator _mediator;

    public LinkController(IMediator mediator) => _mediator = mediator;

    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] GetLinkQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateLink(CreateLinkCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateLink(UpdateLinkCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteLink(DeleteLinkCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
