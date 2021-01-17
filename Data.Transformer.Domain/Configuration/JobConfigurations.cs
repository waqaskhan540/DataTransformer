using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Configuration
{
    public class JsonJobConfig : IJobConfig
    {
        public string SourcePath { get; set; }
        public string ProcessingPath { get; set; }
        public string FailedPath { get; set; }
        public string ProcessedPath { get; set; }
    }

    public class DbJobConfig : IJobConfig
    {
        public string SourcePath { get; set; }
        public string ProcessingPath { get; set; }
        public string FailedPath { get; set; }
        public string ProcessedPath { get; set; }
    }

    public interface IJobConfig
    {
        string SourcePath { get; set; }
        string ProcessingPath { get; set; }
        string FailedPath { get; set; }
        string ProcessedPath { get; set; }
    }
}
