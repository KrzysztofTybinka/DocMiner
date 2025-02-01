using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.FileStorageService
{
    public class FileStorageFactory
    {
        private readonly IOptions<FileStorageSettings> _fileStorageSettings;

        public FileStorageFactory(IOptions<FileStorageSettings> fileStorageSettings)
        {
            _fileStorageSettings = fileStorageSettings;
        }

        public IFileStorageService CreateFileStorageService()
        {
            switch (_fileStorageSettings.Value.StorageOption)
            {
                case "Drive":
                    return new DriveFileStorageService(_fileStorageSettings.Value.Location);

                default:
                    throw new ArgumentException($"Storage type of type: '{_fileStorageSettings.Value.StorageOption}' is not supported.");
            }
        }
    }
}
