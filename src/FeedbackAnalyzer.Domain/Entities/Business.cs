namespace FeedbackAnalyzer.Domain.Entities;

public class Business
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private readonly List<Feedback> _feedbacks = [];
    public IReadOnlyCollection<Feedback> Feedbacks => _feedbacks.AsReadOnly();

    private Business() { }

    public static Business Create(string name, string category, string city, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);

        return new Business
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = category,
            City = city,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddFeedback(Feedback feedback) => _feedbacks.Add(feedback);
}
