namespace FeedbackAnalyzer.Application.DTOs;

public record FeedbackDto(
    Guid Id,
    Guid BusinessId,
    string AuthorName,
    string Text,
    int Rating,
    DateTime SubmittedAt,
    SentimentResultDto? Analysis
);

public record SentimentResultDto(
    string Sentiment,
    float Confidence,
    IReadOnlyList<KeywordDto> Keywords
);

public record KeywordDto(string Word, string Category);
