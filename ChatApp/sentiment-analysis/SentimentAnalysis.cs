using OllamaSharp;
using Microsoft.Extensions.AI;

namespace ChatApp.sentiment_analysis
{
    public class SentimentAnalysis
    {
        public async static Task AnalyseSentiment()
        {

            IChatClient chatClient = new OllamaApiClient(new Uri("http://127.0.0.1:11434/"), "phi3:mini");

            try
            {
                while (true)
                {
                    Console.WriteLine("Please enter the text to analyze the sentiment...");
                    string userPrompt = Console.ReadLine();

                    Console.WriteLine("AI response:");
                    var response = string.Empty;

                    var clientResponse = await chatClient.GetResponseAsync<SentimentRecord>(userPrompt);
                    Console.WriteLine($"Sentiment seems to be {clientResponse.Result.SentimentReview} because {clientResponse.Result.SentimentExplanation}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
