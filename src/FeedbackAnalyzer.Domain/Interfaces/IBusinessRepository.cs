using FeedbackAnalyzer.Domain.Entities;

namespace FeedbackAnalyzer.Domain.Interfaces;

public interface IBusinessRepository
{
    Task<IEnumerable<Business>> GetAllAsync(CancellationToken ct = default);
    Task<Business?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Business business, CancellationToken ct = default);
}
