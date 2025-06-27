using Microsoft.Extensions.AI;
using OllamaSharp;

namespace ChatApp.chat
{
    public class ChatWithOllama
    {
        public static async Task ChatWithBotAsync()
        {
            IChatClient chatClient = new OllamaApiClient(new Uri("http://127.0.0.1:11434/"), "phi3:mini");
            List<ChatMessage> chatHistory = new();

            while (true)
            {
                Console.WriteLine("Your Prompt...");
                var userPrompt = Console.ReadLine();
                chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

                Console.WriteLine("AI response:");
                var response = string.Empty;

                await foreach (ChatResponseUpdate responseUpdate in chatClient.GetStreamingResponseAsync(chatHistory))
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
