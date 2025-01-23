using APIRestIndotInventarioMovil.DTO;
using APIRestIndotInventarioMovil.Models;

namespace APIRestIndotInventarioMovil.Utils
{
    public static class Utilidades
    {
        public static UsuarioApiDTO convertirDTO(this UsuarioSQLServer usuarioSQLServer)
        {
            if (usuarioSQLServer != null)
            {
                return new UsuarioApiDTO
                {
                    Email = usuarioSQLServer.Email,
                    Token = usuarioSQLServer.Token,
                    CodEmpresa = usuarioSQLServer.CodEmpresa
                };
            }

            return null;
        }
    }


}