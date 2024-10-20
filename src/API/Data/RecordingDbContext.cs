using Microsoft.EntityFrameworkCore;
using Recordings.API.Data.Entities;

namespace Recordings.API.Data
{
    public class RecordingDbContext : DbContext
    {
        public DbSet<Recording> Recordings { get; set; }

        public RecordingDbContext(DbContextOptions<RecordingDbContext> options) : base(options)
        {
            // Console.WriteLine("RecordingDbContext");
        }
    }
}