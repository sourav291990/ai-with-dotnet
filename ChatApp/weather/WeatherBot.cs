using Microsoft.Extensions.AI;
using OllamaSharp;

namespace ChatApp.weather
{
    internal class WeatherBot
    {
        string GetCurrentWeather() => Random.Shared.NextDouble() > 0.5 ? "It's sunny" : "It's raining";

        public async Task GetWeatherUpdateAsync()
        {
            IChatClient weatherClient = new OllamaApiClient(new Uri("http://127.0.0.1:11434/"), "phi3:mini");
            weatherClient = ChatClientBuilderChatClientExtensions
                .AsBuilder(weatherClient)
                .UseFunctionInvocation()
                .Build();
            List<ChatMessage> chatHistory = new();


            ChatOptions chatOptions = new ChatOptions
            {
                Tools = [AIFunctionFactory.Create(GetCurrentWeather)]
            };

            while (true)
            {
                Console.WriteLine("Ask me anything on the weather...");
                var userPrompt = Console.ReadLine();
                chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

                Console.WriteLine("AI response:");
                var response = string.Empty;

                await foreach (ChatResponseUpdate responseUpdate in weatherClient.GetStreamingResponseAsync(chatHistory, chatOptions))
                {
                    Console.Write(responseUpdate.Text);
                    response += responseUpdate.Text;
                }

                chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
                Console.WriteLine();
            }
        }
    }
}
