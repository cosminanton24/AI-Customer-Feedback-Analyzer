using FeedbackAnalyzer.Domain.Entities;

namespace FeedbackAnalyzer.Domain.Interfaces;

public interface IFeedbackRepository
{
    Task<IEnumerable<Feedback>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct = default);
    Task AddAsync(Feedback feedback, CancellationToken ct = default);
}
