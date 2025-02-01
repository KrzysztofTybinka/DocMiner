using Domain.Abstractions;
using Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.FileStorageService
{
    public class DriveFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public DriveFileStorageService(string storagePath)
        {
            _storagePath = storagePath;
        }

        public Result<string> CreateStorage(string storageName)
        {
            if (!Directory.Exists(_storagePath))
            {
                return Result<string>.Failure(DriveFileStorageServiceErrors.NotExistingDirectory);
            }

            string path = Path.Combine(_storagePath, storageName);
            Directory.CreateDirectory(path);
            return Result<string>.Success(path);
        }

        public Task<Result> DeleteAsync(string storageLocation)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteStorageAsync(string storageName)
        {
            throw new NotImplementedException();
        }

        public Task<Result<FileInfo>> GetAsync(string storageLocation)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(FileInfo file)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> UploadAsync(FileInfo file)
        {
            throw new NotImplementedException();
        }
    }
}
