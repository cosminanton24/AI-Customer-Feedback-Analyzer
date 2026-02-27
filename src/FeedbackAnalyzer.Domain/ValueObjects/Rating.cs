namespace FeedbackAnalyzer.Domain.ValueObjects;

public sealed class Rating
{
    public int Value { get; }

    private Rating(int value) => Value = value;

    public static Rating Create(int value)
    {
        if (value < 1 || value > 5)
            throw new ArgumentOutOfRangeException(nameof(value), "Rating-ul trebuie sa fie intre 1 si 5.");
        return new Rating(value);
    }

    public override string ToString() => $"{Value}/5";
}
