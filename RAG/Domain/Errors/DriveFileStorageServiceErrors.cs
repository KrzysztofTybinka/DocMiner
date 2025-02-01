using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors
{
    public static class DriveFileStorageServiceErrors
    {
        public static readonly Error NotExistingDirectory = new(
            "DocumentChunk.NotExistingDirectory",
            "Directory doesn't exist."
        );
    }
}
