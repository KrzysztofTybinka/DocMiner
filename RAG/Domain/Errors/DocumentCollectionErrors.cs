using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors
{
    public static class DocumentCollectionErrors
    {
        public static readonly Error NameWithSpaces = new("DocumentCollection.Spaces", 
            "Document collection cannot contain any spaces.");

        public static readonly Error NameEmpty = new("DocumentCollection.EmptyName", 
            "Document collection cannot be empty.");
    }
}
