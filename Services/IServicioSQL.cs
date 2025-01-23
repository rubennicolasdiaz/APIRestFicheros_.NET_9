using APIRestIndotInventarioMovil.Models;

namespace APIRestIndotInventarioMovil.Services
{
    public interface IServicioSQL
    {
        Task<UsuarioSQLServer> DameUsuarioSQLServer(LoginAPI login);
    }
}

