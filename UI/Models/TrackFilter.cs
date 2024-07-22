
namespace Recordings.UI.Models
{
    public class TrackFilter
    {
        public string Value { get; set; } = string.Empty;
        public TrackFilterType Type {get; set; } = TrackFilterType.None;
    }
}
