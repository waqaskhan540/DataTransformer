using Hangfire;
using System;
using System.Threading.Tasks;
using Hangfire.Server;
using Data.Transformer.Domain.BackgroundServices;
using Microsoft.Extensions.Logging;

namespace DataTransformer.Hangfire.Server.Jobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    [AutomaticRetry(Attempts = 3)]
    public class JsonTransformationJob
    {
        private readonly IJsonTransformationService _jsonTransformationService;
        private readonly ILogger<JsonTransformationJob> _logger;
        public JsonTransformationJob(
            IJsonTransformationService jsonTransformationService,
            ILogger<JsonTransformationJob> logger)
        {
            _jsonTransformationService = jsonTransformationService;
            _logger = logger;
        }
        public async Task Execute(PerformContext context)
        {
            

            try
            {
                _logger.LogInformation("starting JsonTransformation Job.");

                await _jsonTransformationService.Start();

                _logger.LogInformation("Finished running JsonTransformation Job.");
            }
            catch (Exception ex)
            {

                _logger.LogError("JsonTransformation Job Failed.",ex);
            } 
        }
    }
}
