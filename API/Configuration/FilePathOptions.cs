
// Non-nullable field must contain a non-null value when
// exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618

namespace Recordings.API.Configuration
{
    public class FilePathOptions
    {
        public string TempPath { get; set; } = "/app/tmp";
        public string StaticFileRootPath { get; set; } = "/app/static";
        public string StaticFileRequestPath { get; set; } = "/static";
    }
}
