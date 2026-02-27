using FeedbackAnalyzer.Application.DTOs;
using FeedbackAnalyzer.Application.Interfaces;
using FeedbackAnalyzer.Domain.Entities;
using FeedbackAnalyzer.Domain.Interfaces;

namespace FeedbackAnalyzer.Application.Services;

public class FeedbackService(
    IFeedbackRepository feedbackRepository,
    IBusinessRepository businessRepository,
    ISentimentAnalyzerService sentimentAnalyzer)
{
    public async Task<FeedbackDto> SubmitFeedbackAsync(
        Guid businessId, string authorName, string text, int rating,
        CancellationToken ct = default)
    {
        var business = await businessRepository.GetByIdAsync(businessId, ct)
            ?? throw new KeyNotFoundException($"Firma cu ID-ul {businessId} nu a fost gasita.");

        var feedback = Feedback.Create(businessId, authorName, text, rating);

        // Analiza AI
        var sentimentResult = await sentimentAnalyzer.AnalyzeAsync(text, ct);
        var analysis = FeedbackAnalysis.Create(
            feedback.Id,
            sentimentResult.Score,
            sentimentResult.Confidence,
            sentimentResult.Keywords);

        feedback.AttachAnalysis(analysis);

        await feedbackRepository.AddAsync(feedback, ct);

        return MapToDto(feedback);
    }

    public async Task<IEnumerable<FeedbackDto>> GetFeedbacksForBusinessAsync(
        Guid businessId, CancellationToken ct = default)
    {
        var feedbacks = await feedbackRepository.GetByBusinessIdAsync(businessId, ct);
        return feedbacks.Select(MapToDto);
    }

    private static FeedbackDto MapToDto(Feedback f)
    {
        SentimentResultDto? analysisDto = null;
        if (f.Analysis is not null)
        {
            analysisDto = new SentimentResultDto(
                f.Analysis.Sentiment.ToString(),
                f.Analysis.Confidence,
                f.Analysis.Keywords
                    .Select(k => new KeywordDto(k.Word, k.Category.ToString()))
                    .ToList()
                    .AsReadOnly()
            );
        }

        return new FeedbackDto(
            f.Id,
            f.BusinessId,
            f.AuthorName,
            f.Text,
            f.Rating.Value,
            f.SubmittedAt,
            analysisDto
        );
    }
}
