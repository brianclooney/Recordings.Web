
// Non-nullable field must contain a non-null value when
// exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618

namespace Recordings.API.DTOs
{
    public class RecordingDto
    {
        public string Title { get; set; }
        public string Mp3 { get; set; }
        public string Date { get; set; }
        public int Duration { get; set; }
    }
}
