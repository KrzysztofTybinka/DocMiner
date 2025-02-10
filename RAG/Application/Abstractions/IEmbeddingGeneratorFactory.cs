using Domain.Embedings;

namespace Application.Abstractions
{
    public interface IEmbeddingGeneratorFactory
    {
        IEmbeddingGenerator CreateEmbeddingGenerator();
    }
}
