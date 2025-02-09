using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Embedings
{
    public static class EmbeddingErrors
    {
        public static Error TextEmpty => Error.Failure(
            "EmbeddingErrors.TextEmpty",
            "To create an embedding you need to provide text it is based on."
        );

        public static Error EmbedingEmpty => Error.Failure(
            "EmbeddingErrors.EmbedingEmpty",
            "Vector embedding was empty."
        );
    }
}
