using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.FileStorage
{
    public interface IFileStorage
    {
        /// <summary>
        /// copy file to source path 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<string> StoreFile(IFormFile file, string filename);

        /// <summary>
        /// moves file to processing folder
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string MoveFileToProcessing(string filename);

        /// <summary>
        /// Returns path to the  folder for processed files
        /// </summary>
        /// <returns></returns>
        string GetPathToProcessedFolder();

        /// <summary>
        /// Remove file from processing folder after finishing job
        /// </summary>
        /// <param name="filename"></param>
        void RemoveFileFromProcessing(string filename);

        /// <summary>
        /// moves file to the failed folder if failed processing
        /// </summary>
        /// <param name="filename"></param>
        void MoveFileToFailed(string filename);

        /// <summary>
        /// Get all files waiting to be processed
        /// </summary>
        /// <returns></returns>
        string[] GetFilesToProcess();
    }
}
