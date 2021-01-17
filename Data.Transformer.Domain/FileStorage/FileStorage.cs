using Data.Transformer.Domain.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.FileStorage
{
    public abstract class FileStorage : IFileStorage
    {
        private readonly IJobConfig _jobConfig;
        
        public FileStorage(IJobConfig jobConfig)
        {
            _jobConfig = jobConfig;
            EnsureDirectoriesCreated();
        }

        
        public string GetPathToProcessedFolder()
        {
            return _jobConfig.ProcessedPath;
        }

        public void MoveFileToFailed(string filename)
        {
            var src = $"{_jobConfig.ProcessingPath}\\{filename}";
            var dest = $"{_jobConfig.FailedPath}\\{filename}";
            File.Move(src, dest);
        }

        public string[] GetFilesToProcess()
        {
            var src = $"{_jobConfig.SourcePath}";
            var files = Directory.GetFiles(src, "*.csv");

            //return file names only
            return files
                    .Select(f => f.Substring(f.LastIndexOf("\\") + 1))
                    .ToArray();
        }
        public string MoveFileToProcessing(string filename)
        {
            var src = $"{_jobConfig.SourcePath}\\{filename}";
            var dest = $"{_jobConfig.ProcessingPath}\\{filename}";

            File.Move(src, dest);

            return dest;
        }

        public void RemoveFileFromProcessing(string filename)
        {
            var path = $"{_jobConfig.ProcessingPath}\\{filename}";
            File.Delete(path);
        }

        public async Task<string> StoreFile(IFormFile file, string filename)
        {
            string path = $"{_jobConfig.SourcePath}\\{filename}";
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return path;
        }

        private void EnsureDirectoriesCreated()
        {
            var srcDirectory = _jobConfig.SourcePath;
            var processingDirectory = _jobConfig.ProcessingPath;
            var processedDirectory = _jobConfig.ProcessedPath;
            var failedDirectory = _jobConfig.FailedPath;

            if (!Directory.Exists(srcDirectory))
                Directory.CreateDirectory(srcDirectory);

            if (!Directory.Exists(processingDirectory))
                Directory.CreateDirectory(processingDirectory);

            if (!Directory.Exists(processedDirectory))
                Directory.CreateDirectory(processedDirectory);

            if (!Directory.Exists(failedDirectory))
                Directory.CreateDirectory(failedDirectory);
        }
    }
}
