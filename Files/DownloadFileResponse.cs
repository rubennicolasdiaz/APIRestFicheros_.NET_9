using Elasticsearch.Net;

namespace APIRestIndotInventarioMovil.Files;

public sealed record DownloadFileResponse(Byte[] bytes, string filename);
