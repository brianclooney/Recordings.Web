using Recordings.API.Configuration;
using Recordings.API.Data;

namespace Recordings.API.Services
{
    public interface IRecordingExtractionService
    {
        Task<Manifest> ProcessRecordingUpload(IFormFile archive, FilePathOptions filePathOptions);
    }
}
