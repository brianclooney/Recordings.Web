using System.ComponentModel.DataAnnotations.Schema;

// Non-nullable field must contain a non-null value when
// exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618

namespace Recordings.API.Data.Entities
{
    public class Recording
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? Notes { get; set; }

        public string FilePath { get; set; }

        public int Duration { get; set; }

        [Column(TypeName = "date")]
        public DateTime RecordingDate { get; set; }

        public int OrdinalNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}