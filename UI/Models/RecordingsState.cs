
using Recordings.Shared.DTOs;

namespace Recordings.UI.Models
{
    public class RecordingsState
    {
        private TrackFilter _trackFilter = new TrackFilter();
        public TrackFilter Filter
        {
            get => _trackFilter;
            set
            {
                if (_trackFilter != value)
                {
                    _trackFilter = value;
                    OnTrackFilterChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private RecordingDto? _selectedTrack;
        public RecordingDto? SelectedTrack 
        { 
            get => _selectedTrack; 
            set
            {
                if (_selectedTrack != value) 
                {
                    _selectedTrack = value;
                    OnSelectedTrackChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? OnTrackFilterChanged;
        public event EventHandler? OnSelectedTrackChanged;
    }
}
