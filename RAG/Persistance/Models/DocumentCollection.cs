using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models
{
    public class DocumentCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public IEnumerable<ProcessedDocument> ProcessedDocuments { get; set; }
    }
}
