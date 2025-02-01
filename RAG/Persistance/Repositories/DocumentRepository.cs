using Domain.Abstractions;
using Domain.Document;
using Persistance.Database;
using Persistance.Services.FileStorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private IFileStorageService _fileStorageService;
        private readonly DocMinerDbContext _context;

        public DocumentRepository(IFileStorageService fileStorageService, DocMinerDbContext context)
        {
            _fileStorageService = fileStorageService;
            _context = context;
        }

        public Task<Result> Create(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
