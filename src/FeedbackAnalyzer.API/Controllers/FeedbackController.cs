using FeedbackAnalyzer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAnalyzer.API.Controllers;

public record SubmitFeedbackRequest(
    string AuthorName,
    string Text,
    int Rating
);

[ApiController]
[Route("api/businesses/{businessId:guid}/feedback")]
[Produces("application/json")]
public class FeedbackController(FeedbackService feedbackService) : ControllerBase
{
    /// <summary>Returneaza toate feedback-urile pentru o firma.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForBusiness(Guid businessId, CancellationToken ct)
    {
        var feedbacks = await feedbackService.GetFeedbacksForBusinessAsync(businessId, ct);
        return Ok(feedbacks);
    }

    /// <summary>
    /// Trimite un feedback nou pentru o firma.
    /// AI-ul analizeaza automat textul si returneaza sentimentul + cuvinte cheie.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Submit(
        Guid businessId,
        [FromBody] SubmitFeedbackRequest request,
        CancellationToken ct)
    {
        try
        {
            var result = await feedbackService.SubmitFeedbackAsync(
                businessId, request.AuthorName, request.Text, request.Rating, ct);

            return CreatedAtAction(nameof(GetForBusiness), new { businessId }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
