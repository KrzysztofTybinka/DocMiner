using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.FileStorageService
{
    public interface IFileStorageService
    {
        Task<Result<string>> UploadAsync(FileInfo file);
        Task<Result> DeleteAsync(string storageLocation);
        Task<Result<FileInfo>> GetAsync(string storageLocation);
        Task<Result> UpdateAsync(FileInfo file);
        Result<string> CreateStorage(string storageName);
        Task<Result> DeleteStorageAsync(string storageName);
    }
}
