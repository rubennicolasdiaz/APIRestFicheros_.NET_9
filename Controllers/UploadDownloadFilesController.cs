using APIRestIndotInventarioMovil.Files;
using APIRestIndotInventarioMovil.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRestIndotInventarioMovil.Controllers;

[Route("apiindot")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class UploadDownloadFilesController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUploadDownloadFileService _fileUploadService;

    public UploadDownloadFilesController(IUploadDownloadFileService fileUploadService, IWebHostEnvironment webHostEnvironment)
    {
        _fileUploadService = fileUploadService;
        _webHostEnvironment = webHostEnvironment; 
    }

    //////////////SUBIR FICHEROS M�LTIPLES//////////////
    [HttpPost("uploadfiles")]
    [Authorize]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
        var result = await _fileUploadService.UploadMultipleFilesAsync(new UploadMultipleFilesRequest(files));
        return Ok(result);
    }

    //////////////SUBIR UN S�LO FICHERO//////////////
    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var result = await _fileUploadService.UploadFileAsync(new UploadFileRequest(file));
        return Ok(result);
    }

    //////////////DESCARGAR VARIOS FICHEROS POR POST//////////////
    [HttpPost("downloadfiles")]
    [Authorize]
    public async Task <IActionResult> DownloadFilesList(List<string> ficheros)
    {
        var result = await _fileUploadService.DownloadMultipleFilesAsync(new DownloadMultipleFilesRequest(ficheros));
        return Ok(result);
    }  

    //////////////DESCARGAR UN S�LO FICHERO POR POST//////////////
    [HttpPost("download")]
    [Authorize]
    public async Task<IActionResult> DownloadFile(string nombreFichero)
    {
        var result = await _fileUploadService.DownloadFileAsync(new DownloadFileRequest(nombreFichero));
        var file = File(result.bytes, "application/octet-stream", result.filename);

        return file; 
        //return Ok(file);
    }

    //////////////ELIMINAR UN S�LO FICHERO//////////////
    [HttpDelete("delete")]
    [Authorize]
    public IActionResult DeleteFile(string fileName)
    {
        var result = _fileUploadService.DeleteFile(new DeleteFileRequest(fileName));
        return Ok(result);
    }
}