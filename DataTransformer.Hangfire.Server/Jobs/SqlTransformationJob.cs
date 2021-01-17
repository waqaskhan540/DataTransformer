using Data.Transformer.Domain.BackgroundServices;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataTransformer.Hangfire.Server.Jobs
{
    public class SqlTransformationJob
    {
        private readonly ISqlTransformationService _sqlTransformationService;
        private readonly ILogger<SqlTransformationJob> _logger;
        public SqlTransformationJob(
            ISqlTransformationService sqlTransformationService
            , ILogger<SqlTransformationJob> logger)
        {
            _sqlTransformationService = sqlTransformationService;
            _logger = logger;
        }
        public async Task Execute(PerformContext context)
        {
            try
            {
                _logger.LogInformation("starting SqlTransformation Job.");

                await _sqlTransformationService.Start();

                _logger.LogInformation("SqlTransformation Job Finished.");
            }
            catch (Exception ex)
            {

                _logger.LogError("SqlTransformationJob failed.", ex);
            }
        }
    }
}
