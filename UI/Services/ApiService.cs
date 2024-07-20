
using Recordings.Shared.DTOs;

namespace Recordings.UI.Services
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
            return await _httpClient.GetFromJsonAsync<List<string>>("/api/recording/dates") ?? new List<string>();
        }

        public async Task<List<RecordingDto>> GetRecordingsForDate(string date)
        {
            try
            {
                var tracks = await _httpClient.GetFromJsonAsync<List<RecordingDto>>($"/api/recording?date={date}") ?? new List<RecordingDto>();
                tracks.ForEach(t => t.Url = new Uri(_httpClient.BaseAddress!, t.Url).ToString());
                return tracks;
            }
            catch (Exception e)
            {
                return new List<RecordingDto>();
            }
        }
    }
}
