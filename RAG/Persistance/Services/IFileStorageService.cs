using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file);
        Task<bool> DeleteAsync(string storageLocation);
    }
}
