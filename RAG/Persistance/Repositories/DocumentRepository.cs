using Domain.Abstractions;
using Domain.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        public Task<Result> Create(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
