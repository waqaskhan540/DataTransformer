using Data.Transformer.Domain.Events;
using Data.Transformer.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Data.Transformer.Api.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IMediator _mediator;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(
              IFileUploadService fileUploadService
            , IMediator mediator
            , ILogger<FileUploadController> logger)
        {
            _fileUploadService = fileUploadService;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                _logger.LogInformation("file upload request received.");

                string filename = await _fileUploadService.UploadFileAsync(file);

                await _mediator.Publish(new FileUploadedEvent
                {
                    FileName = filename
                });

                _logger.LogInformation("file uploaded successfully.");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("file upload request failed.", e);
                return BadRequest(e.Message);
            }
        }
    }
}
