using APIRestIndotInventarioMovil.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace APIRestIndotInventarioMovil.Services
{
    public class ServicioSQL : IServicioSQL

    {
        private const string CADENA_CONEXION_SQL_SERVER = "SQLServer";
        private string cadenaConexionBD;
        private readonly ILogger<IServicioSQL> log;
        public ServicioSQL(IConfiguration configuration, ILogger<IServicioSQL> logger)
        {
            cadenaConexionBD = configuration.GetConnectionString(CADENA_CONEXION_SQL_SERVER);
            log = logger;
        }

        private SqlConnection conexionSQL()
        {
            return new SqlConnection(cadenaConexionBD);
        }

        public async Task<UsuarioSQLServer> DameUsuarioSQLServer(LoginAPI login)
        {

            SqlConnection sqlConexion = conexionSQL();
            UsuarioSQLServer usuarioAPI = null;
            try
            {
                sqlConexion.Open();
                var param = new DynamicParameters();
                param.Add("@Email", login.Email, DbType.String, ParameterDirection.Input, 500);
                param.Add("@Password", login.Password, DbType.String, ParameterDirection.Input, 500);
                usuarioAPI = await sqlConexion.QueryFirstOrDefaultAsync<UsuarioSQLServer>("UsuarioAPIObtener", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error al obtener datos del usuario de login: " + ex.Message);
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return usuarioAPI;
        }
    }
}
