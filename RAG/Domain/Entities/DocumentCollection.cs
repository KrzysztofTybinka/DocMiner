using Domain.Abstractions;
using Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class DocumentCollection
    {
        public Guid Id { get; }
        public string Name { get; }

        private DocumentCollection(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Result<DocumentCollection> Create(Guid id, string name)
        {
            if (name.Contains(' '))
            {
                return Result<DocumentCollection>.Failure(DocumentCollectionErrors.NameWithSpaces);
            }
            if (string.IsNullOrEmpty(name))
            {
                return Result<DocumentCollection>.Failure(DocumentCollectionErrors.NameEmpty);
            }
            
            var result = new DocumentCollection(id, name);
            return Result<DocumentCollection>.Success(result);
        }
    }
}
