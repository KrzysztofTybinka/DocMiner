using Domain.Abstractions;
using Domain.DocumentCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DocumentCollectionService
    {
        private IDocumentCollectionRepository _documentCollectionRepository;

        public DocumentCollectionService(IDocumentCollectionRepository documentCollectionRepository)
        {
            _documentCollectionRepository = documentCollectionRepository;
        }

        public async Task<Result> CreateAsync(string collectionName)
        {
            return await _documentCollectionRepository.Create(collectionName);
        }
    }
}
