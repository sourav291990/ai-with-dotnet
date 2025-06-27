namespace ChatApp.sentiment_analysis
{
    public record SentimentRecord(string SentimentExplanation, Sentiment SentimentReview);
    public enum Sentiment
    {
        Positive,
        Negative,
        Neutral
    }
}
