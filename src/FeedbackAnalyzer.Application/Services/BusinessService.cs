using FeedbackAnalyzer.Application.DTOs;
using FeedbackAnalyzer.Domain.Interfaces;

namespace FeedbackAnalyzer.Application.Services;

public class BusinessService(IBusinessRepository businessRepository)
{
    public async Task<IEnumerable<BusinessDto>> GetAllAsync(CancellationToken ct = default)
    {
        var businesses = await businessRepository.GetAllAsync(ct);
        return businesses.Select(b => new BusinessDto(
            b.Id,
            b.Name,
            b.Category,
            b.City,
            b.Description,
            b.Feedbacks.Count
        ));
    }

    public async Task<BusinessDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var business = await businessRepository.GetByIdAsync(id, ct);
        if (business is null) return null;

        return new BusinessDto(
            business.Id,
            business.Name,
            business.Category,
            business.City,
            business.Description,
            business.Feedbacks.Count
        );
    }
}
