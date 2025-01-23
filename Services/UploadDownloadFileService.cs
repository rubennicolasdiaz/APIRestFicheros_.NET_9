using APIRestIndotInventarioMovil.Files;

namespace APIRestIndotInventarioMovil.Services;

public sealed class UploadDownloadFileService : IUploadDownloadFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public UploadDownloadFileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<UploadMultipleFilesResponse> UploadMultipleFilesAsync(UploadMultipleFilesRequest request)
    {
        var uploadedFilePaths = new List<string>();
        
        // Iteramos sobre todos los archivos
        foreach (var file in request.FilesUrl)
        {
            // Usamos solo 'Uploads' como carpeta raíz, sin subcarpetas basadas en el nombre del archivo
            string folderPath = Path.Combine("Uploads");

            // Crear la carpeta si no existe
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Extraer la extensión del archivo para validarlo (en este ejemplo, JSON)
            string extension = Path.GetExtension(file.FileName);

            // Si no es un archivo JSON válido, omitimos el archivo
            if (!IsValidJsonFile(extension))
            {
                continue; // El archivo no es válido, se omite.
            }

            string fileNameWithExtension = Path.GetFileName(file.FileName);

            // Combinamos la ruta de la carpeta con el nombre del archivo
            string filePath = Path.Combine(folderPath, fileNameWithExtension);

            // Usamos 'using' para asegurarnos de que el FileStream se cierre correctamente
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Copiamos el archivo al disco
                await file.CopyToAsync(stream);
            }

            // Agregar la ruta de archivo subida a la lista
            uploadedFilePaths.Add(fileNameWithExtension); 
        }

        return new UploadMultipleFilesResponse(uploadedFilePaths);
    }

public async Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request)
    {
        // Usamos solo 'Uploads' como carpeta raíz, sin subcarpetas basadas en el nombre del archivo
        string folderPath = Path.Combine("Uploads");

        // Crear la carpeta si no existe
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Extraer la extensión del archivo para validarlo (en este ejemplo, JSON)
        var extension = Path.GetExtension(request.File.FileName);

        // Si no es un archivo JSON válido, omitimos el archivo
        if (!IsValidJsonFile(extension))
        {
            return new UploadFileResponse(string.Empty); // Retorna respuesta vacía si no es válido
        }

        string fileNameWithExtension = Path.GetFileName(request.File.FileName);

        // Combinamos la ruta de la carpeta con el nombre del archivo
        string filePath = Path.Combine(folderPath, fileNameWithExtension);

        // Usamos 'using' para asegurarnos de que el FileStream se cierre correctamente
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        return new UploadFileResponse(fileNameWithExtension); 
    }

    public bool DeleteFile(DeleteFileRequest request)
    {
        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "file");
        var filePath = Path.Combine(folderPath, request.FileName);

        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            File.Delete(filePath);
            return true; 
        }
        catch (IOException ex)
        {
            
           
            return false;
        }
    }


    private bool IsValidJsonFile(string extension)
    {
        string jsonExtension = ".json";

        if (extension.Equals(jsonExtension))
        {
            return true; 
        }
        return false; 
    }

    public async Task<DownloadMultipleFilesResponse> DownloadMultipleFilesAsync(DownloadMultipleFilesRequest request)
    {

        string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Downloads";

        List<Byte[]> listadoFicheros = new List<Byte[]>(); 
        List<String> listadoURLs = new List<String>();  

            foreach (var file in request.FilesUrl)
        {

            string rutaDestinoCompleta = Path.Combine(rutaDestino, file);
            byte[] bytes = await File.ReadAllBytesAsync(rutaDestinoCompleta);

            listadoFicheros.Add(bytes);
            listadoURLs.Add(rutaDestinoCompleta); 

        }
        
        return new DownloadMultipleFilesResponse(listadoFicheros, listadoURLs);
    }

    public async Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request)
    {
        string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Downloads";
        string rutaDestinoCompleta = Path.Combine(rutaDestino, request.filename);

        byte[] bytes = await File.ReadAllBytesAsync(rutaDestinoCompleta);
        
        return new DownloadFileResponse(bytes, request.filename);
    }
}