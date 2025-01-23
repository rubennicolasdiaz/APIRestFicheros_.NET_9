namespace APIRestIndotInventarioMovil.Files;

public sealed record DownloadMultipleFilesResponse(List<Byte[]> listadoFicheros, List<String> filesURLs);

