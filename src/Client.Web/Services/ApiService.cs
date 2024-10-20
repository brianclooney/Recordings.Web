
using Recordings.Shared.DTOs;

namespace Recordings.Client.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<string>> GetRecordingDatesAsync()
        {
	    try {
                return await _httpClient.GetFromJsonAsync<List<string>>("/api/recording/dates") ?? new List<string>();
	    }
	    catch (Exception e)
	    {
	        _logger.LogError($"GetRecordingDatesAsync {e.Message}");
                return new List<string>();
	    }
        }

        public async Task<List<RecordingDto>> GetRecordingsByDate(string date)
        {
	    try {
                return await GetRecordings($"/api/recording?date={date}");
	    }
	    catch (Exception e)
	    {
	        _logger.LogError($"GetRecordingsByDate {e.Message}");
		return new List<RecordingDto>();
	    }
        }

        public async Task<List<RecordingDto>> GetRecordingsByTitle(string searchString)
        {
	    try {
                return await GetRecordings($"/api/recording?title={searchString}");
	    }
	    catch (Exception e)
	    {
	        _logger.LogError($"GetRecordingsByTitle {e.Message}");
		return new List<RecordingDto>();
	    }
        }

        private async Task<List<RecordingDto>> GetRecordings(string uri)
        {
            try
            {
                var tracks = await _httpClient.GetFromJsonAsync<List<RecordingDto>>(uri) ?? new List<RecordingDto>();
                // tracks.ForEach(t => t.Url = new Uri(_httpClient.BaseAddress!, t.Url).ToString());
                // tracks.ForEach(t => t.Url = new Uri(t.Url).ToString());
                return tracks;
            }
            catch (Exception e)
            {
	        _logger.LogError($"GetRecordings {e.Message}");
                return new List<RecordingDto>();
            }
        }
    }
}
