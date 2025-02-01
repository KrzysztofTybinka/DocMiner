using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Embedding
{
    public interface IEmbeddingRepository
    {
        Task<Result<Embedding>> Get(IEnumerable<float> embeddingVector, string chunk);
    }
}
