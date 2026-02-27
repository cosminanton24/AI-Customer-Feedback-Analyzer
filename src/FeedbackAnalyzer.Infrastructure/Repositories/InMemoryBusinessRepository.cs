using FeedbackAnalyzer.Domain.Entities;
using FeedbackAnalyzer.Domain.Interfaces;

namespace FeedbackAnalyzer.Infrastructure.Repositories;

public class InMemoryBusinessRepository : IBusinessRepository
{
    private readonly List<Business> _businesses = [];

    public Task<IEnumerable<Business>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<Business>>(_businesses);

    public Task<Business?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_businesses.FirstOrDefault(b => b.Id == id));

    public Task AddAsync(Business business, CancellationToken ct = default)
    {
        _businesses.Add(business);
        return Task.CompletedTask;
    }

    public void Seed(IEnumerable<Business> businesses) => _businesses.AddRange(businesses);
}
