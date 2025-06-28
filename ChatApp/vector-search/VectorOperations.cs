using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OpenAI;
using System.ClientModel;
using System.Reflection.Emit;

namespace ChatApp.vector_search
{
    internal class VectorOperations
    {


        private readonly IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        private readonly string _openAIKey;
        private readonly string _embeddingModelName;
        private VectorStoreCollection<int, CloudService> _cloudServiceStore;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;

        public VectorOperations()
        {
            _embeddingModelName = config["EmbeddingModelName"];
            _openAIKey = config["OpenAIKey"];
            _generator = new OpenAIClient(new ApiKeyCredential(_openAIKey))
                .GetEmbeddingClient(_embeddingModelName)
                .AsIEmbeddingGenerator();

        }

        public async Task GenerateInitialVectorAsync(string vectorStoreName, List<CloudService> cloudServices)
        {
            var vectorStore = new InMemoryVectorStore();
            _cloudServiceStore = vectorStore.GetCollection<int, CloudService>(vectorStoreName);
            await _cloudServiceStore.EnsureCollectionExistsAsync();

            foreach (CloudService service in cloudServices)
            {
                service.Vector = await _generator.GenerateVectorAsync(service.Description);
                await _cloudServiceStore.UpsertAsync(service);
            }
        }

        public async Task<ReadOnlyMemory<float>> GenerateEmbeddingsFromQuery(string query)
        {
            return await _generator.GenerateVectorAsync(query);
        }

        public IAsyncEnumerable<VectorSearchResult<CloudService>> Search(ReadOnlyMemory<float> queryEmbedding)
        {
            return _cloudServiceStore.SearchAsync(queryEmbedding, top: 1);
        }
    }
}
