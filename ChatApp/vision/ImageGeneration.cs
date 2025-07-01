using Microsoft.Extensions.Configuration;
using OpenAI.Images;

namespace ChatApp.vision
{
    internal class ImageGeneration
    {
        private readonly IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        private readonly string _openAiImagegenerationModelName;
        private readonly string _openAIKey;
        public ImageGeneration()
        {
            _openAiImagegenerationModelName = config["OpenAiImagegenerationModelName"];
            _openAIKey = config["OpenAIKey"];
        }
        

        internal void InitiateImageGeneration()
        {
            while (true)
            {
                Console.WriteLine("Enter a prompt for image generation (or type 'exit' to quit):");
                string imageGenerationPrompt = Console.ReadLine();
                if (imageGenerationPrompt?.ToLower() == "exit")
                {
                    break;
                }
                try
                {
                    GenerateImage(imageGenerationPrompt).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while generating the image: {ex.Message}");
                }
            }
        }

        private async Task GenerateImage(string imageGenerationPrompt)
        {
            ImageClient imageClient = new(_openAiImagegenerationModelName, _openAIKey);

            GeneratedImage generatedImage = await imageClient.GenerateImageAsync(imageGenerationPrompt,
                new ImageGenerationOptions { Size = GeneratedImageSize.W1024xH1024 });

            Console.WriteLine($"The generated image is ready at:\n{generatedImage.ImageUri}");
        }
    }
}
