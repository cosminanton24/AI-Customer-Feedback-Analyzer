using FeedbackAnalyzer.Application.Interfaces;
using FeedbackAnalyzer.Domain.ValueObjects;

namespace FeedbackAnalyzer.Infrastructure.AI;

/// <summary>
/// Analizor simplu bazat pe cuvinte cheie - inlocuit cu OpenAI in Phase 3.
/// </summary>
public class MockSentimentAnalyzer : ISentimentAnalyzerService
{
    private static readonly Dictionary<string, SentimentScore> SentimentWords = new(StringComparer.OrdinalIgnoreCase)
    {
        // Pozitive
        ["excelent"] = SentimentScore.Positive,
        ["grozav"] = SentimentScore.Positive,
        ["perfect"] = SentimentScore.Positive,
        ["multumit"] = SentimentScore.Positive,
        ["bun"] = SentimentScore.Positive,
        ["buna"] = SentimentScore.Positive,
        ["rapid"] = SentimentScore.Positive,
        ["recomandat"] = SentimentScore.Positive,
        ["super"] = SentimentScore.Positive,
        ["fantastic"] = SentimentScore.Positive,
        ["recomand"] = SentimentScore.Positive,
        // Negative
        ["rau"] = SentimentScore.Negative,
        ["dezamagit"] = SentimentScore.Negative,
        ["dezamagitor"] = SentimentScore.Negative,
        ["lent"] = SentimentScore.Negative,
        ["murdar"] = SentimentScore.Negative,
        ["nepoliticos"] = SentimentScore.Negative,
        ["scump"] = SentimentScore.Negative,
        ["groaznic"] = SentimentScore.Negative,
        ["inacceptabil"] = SentimentScore.Negative,
        ["trist"] = SentimentScore.Negative,
    };

    private static readonly Dictionary<string, KeywordCategory> KeywordMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["serviciu"] = KeywordCategory.Service,
        ["servicii"] = KeywordCategory.Service,
        ["mancare"] = KeywordCategory.Service,
        ["meniu"] = KeywordCategory.Service,
        ["locatie"] = KeywordCategory.Location,
        ["parcare"] = KeywordCategory.Location,
        ["adresa"] = KeywordCategory.Location,
        ["acces"] = KeywordCategory.Location,
        ["pret"] = KeywordCategory.Price,
        ["ieftin"] = KeywordCategory.Price,
        ["scump"] = KeywordCategory.Price,
        ["tarif"] = KeywordCategory.Price,
        ["personal"] = KeywordCategory.Staff,
        ["angajat"] = KeywordCategory.Staff,
        ["chelner"] = KeywordCategory.Staff,
        ["politicos"] = KeywordCategory.Staff,
        ["nepoliticos"] = KeywordCategory.Staff,
        ["atmosfera"] = KeywordCategory.Ambiance,
        ["ambient"] = KeywordCategory.Ambiance,
        ["muzica"] = KeywordCategory.Ambiance,
        ["curat"] = KeywordCategory.Ambiance,
        ["murdar"] = KeywordCategory.Ambiance,
    };

    public Task<SentimentResult> AnalyzeAsync(string feedbackText, CancellationToken ct = default)
    {
        var words = feedbackText.Split([' ', ',', '.', '!', '?', '\n'], StringSplitOptions.RemoveEmptyEntries);

        int positiveCount = 0, negativeCount = 0;
        var detectedKeywords = new List<DetectedKeyword>();

        foreach (var word in words)
        {
            if (SentimentWords.TryGetValue(word, out var score))
            {
                if (score == SentimentScore.Positive) positiveCount++;
                else if (score == SentimentScore.Negative) negativeCount++;
            }

            if (KeywordMap.TryGetValue(word, out var category))
            {
                if (!detectedKeywords.Any(k => k.Word.Equals(word, StringComparison.OrdinalIgnoreCase)))
                    detectedKeywords.Add(new DetectedKeyword(word.ToLower(), category));
            }
        }

        var sentiment = (positiveCount, negativeCount) switch
        {
            _ when positiveCount > negativeCount => SentimentScore.Positive,
            _ when negativeCount > positiveCount => SentimentScore.Negative,
            _ => SentimentScore.Neutral
        };

        float total = positiveCount + negativeCount;
        float confidence = total == 0 ? 0.5f : Math.Max(positiveCount, negativeCount) / total;

        return Task.FromResult(new SentimentResult(sentiment, confidence, detectedKeywords.AsReadOnly()));
    }
}
