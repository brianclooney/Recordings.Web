using Recordings.API.Data;
using Recordings.API.Data.Entities;
using Recordings.API.DTOs;

namespace Recordings.API.Extensions
{
    public static class RecordingExtensions
    {
        public static RecordingDto AsDto(this Recording recording, string staticRoot = "")
        {
            return new RecordingDto
            {
                Title = recording.Title,
                Date = recording.RecordingDate.ToString("yyyy-MM-dd"),
                Duration = recording.Duration,
                Mp3 = Path.Combine(staticRoot, recording.FilePath)
            };
        }

        public static Recording ToRecording(this Track track, DateTime dateRecorded, DateTime? dateAdded = null)
        {
            return new Recording
            {
                FilePath = track.File,
                Title = track.Title,
                Duration = track.Duration,
                RecordingDate = dateRecorded,
                OrdinalNumber = track.Index,
                CreatedAt = dateAdded ?? DateTime.Now
            };
        }

        public static List<Recording> ToRecordings(this Manifest manifest)
        {
            var dateRecorded = manifest.DateRecorded;
            var dateAdded = DateTime.Now;
            return manifest.Tracks.Select(t => t.ToRecording(dateRecorded, dateAdded)).ToList();
        }
    }
}
