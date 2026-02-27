namespace FeedbackAnalyzer.Application.DTOs;

public record BusinessDto(
    Guid Id,
    string Name,
    string Category,
    string City,
    string Description,
    int FeedbackCount
);
