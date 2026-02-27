using FeedbackAnalyzer.Domain.Entities;
using FeedbackAnalyzer.Domain.Interfaces;

namespace FeedbackAnalyzer.Infrastructure.Repositories;

public class InMemoryFeedbackRepository : IFeedbackRepository
{
    private readonly List<Feedback> _feedbacks = [];

    public Task<IEnumerable<Feedback>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct = default)
        => Task.FromResult(_feedbacks.Where(f => f.BusinessId == businessId));

    public Task AddAsync(Feedback feedback, CancellationToken ct = default)
    {
        _feedbacks.Add(feedback);
        return Task.CompletedTask;
    }
}
