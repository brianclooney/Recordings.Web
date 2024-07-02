
// Non-nullable field must contain a non-null value when
// exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618

namespace Recordings.API.Data
{
    public class Track
    {
        public int Index { get; set; }
        public string File { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; } // Duration in seconds
    }

    public class Manifest
    {
        public string Title { get; set; }
        public DateTime DateRecorded { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
