using System.IO.Compression;
using System.Text.Json;
using Recordings.API.Configuration;
using Recordings.API.Data;

namespace Recordings.API.Services
{
    public class RecordingExtractionService : IRecordingExtractionService
    {
        public async Task<Manifest> ProcessRecordingUpload(IFormFile archive, FilePathOptions filePathOptions)
        {
            // Ensure the uploaded file is a zip archive
            if (archive == null || !archive.FileName.EndsWith(".zip"))
            {
                throw new ArgumentException("A zip archive is required.");
            }

            var uploadId = Guid.NewGuid().ToString();

            // Define a path to temporarily store the uploaded archive
            var tempFilePath = $"{filePathOptions.TempPath}/{uploadId}.tmp";

            // Save the uploaded archive to the tempFilePath
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await archive.CopyToAsync(stream);
            }

            // Define a directory to extract the archive to
            var extractionPath = $"{filePathOptions.StaticFileRootPath}/{uploadId}";

            // Extract the contents of the archive
            ZipFile.ExtractToDirectory(tempFilePath, extractionPath);

            // Clean up the temporary file
            File.Delete(tempFilePath);

            // Read the metadata file (manifest.json)
            var manifest = ReadManifest(extractionPath);

            if (manifest == null)
            {
                // Clean up the extracted files
                Directory.Delete(extractionPath, true);

                throw new ArgumentException("Unable to read manifest file.");
            }

            string targetDirectoryPath = $"{filePathOptions.StaticFileRootPath}/{manifest.DateRecorded:yyyy-MM-dd}";
            if (Directory.Exists(targetDirectoryPath))
            {
                // Clean up the extracted files
                Directory.Delete(extractionPath, true);

                throw new ArgumentException("Files already exist for that date.");
            }
            else
            {
                // Update the manifest with the static file root path
                Directory.Move(extractionPath, $"{filePathOptions.StaticFileRootPath}/{manifest.DateRecorded:yyyy-MM-dd}");
            }

            // Update the manifest tracks with the static file request path
            foreach (var track in manifest.Tracks)
            {
                track.File = $"{manifest.DateRecorded:yyyy-MM-dd}/{track.File}";
            }

            return manifest;
        }

        private Manifest? ReadManifest(string extractionPath)
        {
            var manifestFilePath = Path.Combine(extractionPath, "manifest.json");
            var manifestJson = File.ReadAllText(manifestFilePath);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return JsonSerializer.Deserialize<Manifest>(manifestJson, options);
        }
    }
}
