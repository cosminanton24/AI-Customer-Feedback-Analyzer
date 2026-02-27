using FeedbackAnalyzer.Domain.ValueObjects;

namespace FeedbackAnalyzer.Domain.Entities;

public class Feedback
{
    public Guid Id { get; private set; }
    public Guid BusinessId { get; private set; }
    public string AuthorName { get; private set; } = string.Empty;
    public string Text { get; private set; } = string.Empty;
    public Rating Rating { get; private set; } = null!;
    public DateTime SubmittedAt { get; private set; }

    public FeedbackAnalysis? Analysis { get; private set; }

    private Feedback() { }

    public static Feedback Create(Guid businessId, string authorName, string text, int ratingValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(authorName);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        if (text.Length < 10)
            throw new ArgumentException("Feedback-ul trebuie sa aiba cel putin 10 caractere.", nameof(text));

        return new Feedback
        {
            Id = Guid.NewGuid(),
            BusinessId = businessId,
            AuthorName = authorName,
            Text = text,
            Rating = Rating.Create(ratingValue),
            SubmittedAt = DateTime.UtcNow
        };
    }

    public void AttachAnalysis(FeedbackAnalysis analysis) => Analysis = analysis;
}
