using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ProcessedDocument
{
    internal interface IProcessedDocumentRepository
    {
        Task<Result<ProcessedDocument>> Create();
    }
}
