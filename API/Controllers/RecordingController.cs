using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recordings.API.Configuration;
using Recordings.API.Data;
using Recordings.Shared.DTOs;
using Recordings.API.Extensions;
using Recordings.API.Services;

namespace Recordings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private readonly RecordingDbContext _context;
        private readonly IRecordingExtractionService _recordingExtractionService;
        private readonly FilePathOptions _filePathOptions;
        private readonly ILogger<RecordingController> _logger;

        public RecordingController(
            RecordingDbContext context,
            IRecordingExtractionService recordingExtractionService,
            IOptions<FilePathOptions> filePathOptions,
            ILogger<RecordingController> logger)
        {
            _filePathOptions = filePathOptions.Value;
            _context = context;
            _recordingExtractionService = recordingExtractionService;
            _logger = logger;
        }

        [HttpGet("dates")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetRecordingDates()
        {
            // Console.WriteLine("GetRecordingDates");
            _logger.LogInformation("GetRecordingDates");

            var dates = await _context.Recordings
                .Select(r => r.RecordingDate)
                .Distinct()
                .OrderByDescending(d => d)
                .ToListAsync();

            return dates.Count == 0 ?
                NotFound() :
                Ok(dates.Select(d => d.ToString("yyyy-MM-dd")));
        }

        [HttpGet("titles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRecordingTitles([FromQuery] string? filterString)
        {
            var query = _context.Recordings.Select(r => r.Title.Replace("*", string.Empty));

            if (!string.IsNullOrEmpty(filterString))
            {
                query = query.Where(t => t.ToLower().Contains(filterString.ToLower()));
            }

            var titles = await query.Distinct().OrderBy(t => t).ToListAsync();

            return titles.Count == 0 ?
                NotFound() :
                Ok(titles);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecordingDto>>> GetRecordings([FromQuery] DateTime? date, [FromQuery] string? title = null)
        {
            var query = _context.Recordings.AsQueryable();

            if (!date.HasValue && string.IsNullOrEmpty(title))
            {
                return BadRequest(new { Message = "The date or title query parameter must be provided." });
            }

            if (date.HasValue)
            {
                query = query.Where(r => r.RecordingDate == date);
            }

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(r => r.Title.ToLower().Contains(title.ToLower()));
            }

            var recordings = await query.OrderByDescending(r => r.RecordingDate).ThenBy(r => r.OrdinalNumber).ToListAsync();

            var staticRoot = $"{_filePathOptions.StaticFileRequestPath}";
            // _logger.LogInformation($"staticRoot = {staticRoot}");

            return recordings.Count == 0 ?
                NotFound() :
                Ok(recordings.Select(r => r.AsDto(staticRoot)));
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)] // 100 MB
        [RequestSizeLimit(104857600)] // 100 MB
        public async Task<IActionResult> PostRecordings(IFormFile archive)
        {
            _logger.LogInformation("PostRecordings");

            try
            {
                Manifest manifest = await _recordingExtractionService.ProcessRecordingUpload(archive, _filePathOptions);
                var recordings = manifest.ToRecordings();
                _context.Recordings.AddRange(recordings);
                _context.SaveChanges();
                return Ok();
            }
            catch (ArgumentException e)
            {
                _logger.LogError("PostRecordings: " + e.Message);
                return BadRequest(new { e.Message });
            }
        }
    }
}
