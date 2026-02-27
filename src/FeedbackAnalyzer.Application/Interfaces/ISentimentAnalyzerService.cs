using FeedbackAnalyzer.Domain.Entities;
using FeedbackAnalyzer.Domain.ValueObjects;

namespace FeedbackAnalyzer.Application.Interfaces;

public record SentimentResult(
    SentimentScore Score,
    float Confidence,
    IReadOnlyList<DetectedKeyword> Keywords
);

public interface ISentimentAnalyzerService
{
    Task<SentimentResult> AnalyzeAsync(string feedbackText, CancellationToken ct = default);
}
