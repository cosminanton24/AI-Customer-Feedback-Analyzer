using FeedbackAnalyzer.Domain.ValueObjects;

namespace FeedbackAnalyzer.Domain.Entities;

public class FeedbackAnalysis
{
    public Guid Id { get; private set; }
    public Guid FeedbackId { get; private set; }
    public SentimentScore Sentiment { get; private set; }
    public float Confidence { get; private set; }
    public IReadOnlyList<DetectedKeyword> Keywords { get; private set; } = [];
    public DateTime AnalyzedAt { get; private set; }

    private FeedbackAnalysis() { }

    public static FeedbackAnalysis Create(
        Guid feedbackId,
        SentimentScore sentiment,
        float confidence,
        IEnumerable<DetectedKeyword> keywords)
    {
        return new FeedbackAnalysis
        {
            Id = Guid.NewGuid(),
            FeedbackId = feedbackId,
            Sentiment = sentiment,
            Confidence = Math.Clamp(confidence, 0f, 1f),
            Keywords = keywords.ToList().AsReadOnly(),
            AnalyzedAt = DateTime.UtcNow
        };
    }
}
