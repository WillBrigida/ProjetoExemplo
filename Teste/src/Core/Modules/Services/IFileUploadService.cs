using Core.Modules.Models;

namespace Core.Modules.Services
{
    public interface IFileUploadService
    {
        Task UploadImagens(object? _event = null, int fileCount = 1);
        Task UploadVideos(object? _event = null, int fileCount = 1);
        Task UploadPDFs(object? _event = null, int fileCount = 1);
        List<FileData> Files { get; set; }
        IList<string> Images { get; set; }
    }
}
