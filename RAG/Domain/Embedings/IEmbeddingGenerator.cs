using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Embedings
{
    public interface IEmbeddingGenerator
    {
        Task<Result<Embedding>> Create(string text, float[] textEmbbeding);
        Task<Result<List<Embedding>>> CreateMany(List<string> textPieces, 
            List<float[]> textEmbeddings);
    }
}
