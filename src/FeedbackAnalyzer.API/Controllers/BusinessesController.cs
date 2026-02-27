using FeedbackAnalyzer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAnalyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BusinessesController(BusinessService businessService) : ControllerBase
{
    /// <summary>Returneaza lista tuturor firmelor locale inregistrate.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var businesses = await businessService.GetAllAsync(ct);
        return Ok(businesses);
    }

    /// <summary>Returneaza detaliile unei firme dupa ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var business = await businessService.GetByIdAsync(id, ct);
        if (business is null)
            return NotFound(new { message = $"Firma cu ID-ul {id} nu a fost gasita." });

        return Ok(business);
    }
}
