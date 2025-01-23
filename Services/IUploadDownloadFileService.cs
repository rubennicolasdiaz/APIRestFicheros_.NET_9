using APIRestIndotInventarioMovil.Files;

namespace APIRestIndotInventarioMovil.Services; 

public interface IUploadDownloadFileService
{
    //UPLOAD FILES:
    Task<UploadMultipleFilesResponse> UploadMultipleFilesAsync(UploadMultipleFilesRequest request);
    Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request);

    //DOWNLOAD FILES:
    Task<DownloadMultipleFilesResponse> DownloadMultipleFilesAsync(DownloadMultipleFilesRequest request);
    Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request);
    
    //DELETE FILE:
    bool DeleteFile(DeleteFileRequest request);
}