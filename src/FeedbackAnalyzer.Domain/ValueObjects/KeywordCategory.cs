namespace FeedbackAnalyzer.Domain.ValueObjects;

public enum KeywordCategory
{
    Service,
    Location,
    Price,
    Staff,
    Ambiance,
    Other
}

public sealed record DetectedKeyword(string Word, KeywordCategory Category);
