using Microsoft.Extensions.VectorData;

namespace ChatApp.vector_search
{
    internal class CloudService
    {
        [VectorStoreKey]
        public int Key { get; set; }
        [VectorStoreData]
        public string Name { get; set; }
        [VectorStoreData]
        public string Description { get; set; }
        [VectorStoreVector(Dimensions: 384, DistanceFunction = DistanceFunction.CosineDistance)]
        public ReadOnlyMemory<float> Vector { get; set; }
    }
}
