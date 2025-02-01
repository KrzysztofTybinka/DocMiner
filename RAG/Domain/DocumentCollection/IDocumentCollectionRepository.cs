using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DocumentCollection
{
    public interface IDocumentCollectionRepository
    {
        Task<Result> Create(string collectionName);
        Task<Result<DocumentCollection>> Get(string collectionName);
        Task<Result<List<DocumentCollection>>> ListAll();
        Task<Result> Delete(string collectionName);

    }
}
