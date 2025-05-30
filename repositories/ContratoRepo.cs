using System.Collections.Generic;
using MySql.Data.MySqlClient;
using InmobiliariaApp.Data;
using InmobiliariaApp.Models;
using System;

namespace InmobiliariaApp.Repositories
{
    public class ContratoRepo
    {
        private readonly DatabaseConnection _dbConnection;

        public ContratoRepo(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Método para obtener todos los contratos
        public List<Contrato> GetAll()
        {
            var contratos = new List<Contrato>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT c.*, CONCAT(i.nombre, ' ', i.apellido) AS nombre_y_apellido, im.direccion, im.portada FROM contrato c JOIN inquilino i ON i.id_inquilino = c.id_inquilino JOIN inmueble im ON c.id_inmueble = im.id_inmueble;", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("id_contrato"),
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    IdInquilino = reader.GetInt32("id_inquilino"),
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32("anulado_por"),
                    FechaInicio = reader.GetDateTime("f_inicio"),
                    FechaFin = reader.GetDateTime("f_fin"),
                    MontoMensual = reader.GetDouble("monto_mensual"),
                    Estado = reader.GetString("estado"),
                    NyAInquilino = reader.GetString("nombre_y_apellido"),
                    Direccion = reader.GetString("direccion"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    
                });
            }

            return contratos;
        }

        // ------ filtrar por inmueble -------------

       public List<Contrato> GetContratosPorInmueble(int idInmueble, int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var contratos = new List<Contrato>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            // Obtener total de registros
            using (var countCmd = new MySqlCommand(@"
                SELECT COUNT(*) 
                FROM contrato c 
                WHERE c.id_inmueble = @IdInmueble;", connection))
            {
                countCmd.Parameters.AddWithValue("@IdInmueble", idInmueble);
                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina - 1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT c.*, CONCAT(i.nombre, ' ', i.apellido) AS NyA_inquilino, im.direccion, im.portada 
                FROM contrato c 
                JOIN inquilino i ON i.id_inquilino = c.id_inquilino 
                JOIN inmueble im ON c.id_inmueble = im.id_inmueble 
                WHERE c.id_inmueble = @IdInmueble 
                LIMIT @limit OFFSET @offset;", connection);

            command.Parameters.AddWithValue("@IdInmueble", idInmueble);
            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("id_contrato"),
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    IdInquilino = reader.GetInt32("id_inquilino"),
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32("anulado_por"),
                    FechaInicio = reader.GetDateTime("f_inicio"),
                    FechaFin = reader.GetDateTime("f_fin"),
                    MontoMensual = reader.GetDouble("monto_mensual"),
                    Estado = reader.GetString("estado"),
                    NyAInquilino = reader.GetString("NyA_inquilino"),
                    Direccion = reader.GetString("direccion"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada")
                });
            }

            return contratos;
        }


        // filtro fecha vigente inicio fin

        public List<Contrato> GetContratosPorFecha(DateTime fechaInicio, DateTime fechaFin, int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var contratos = new List<Contrato>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            // Obtener total de registros
            using (var countCmd = new MySqlCommand(@"
                SELECT COUNT(*) 
                FROM contrato c 
                WHERE c.f_inicio = @FechaInicio AND c.f_fin = @FechaFin AND c.estado = 'vigente';", connection))
            {
                countCmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                countCmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina - 1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT c.*, CONCAT(i.nombre, ' ', i.apellido) AS NyA_inquilino, im.direccion, im.portada  
                FROM contrato c 
                JOIN inquilino i ON i.id_inquilino = c.id_inquilino 
                JOIN inmueble im ON c.id_inmueble = im.id_inmueble 
                WHERE c.f_inicio = @FechaInicio AND c.f_fin = @FechaFin AND c.estado = 'vigente' 
                LIMIT @limit OFFSET @offset;", connection);

            command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
            command.Parameters.AddWithValue("@FechaFin", fechaFin);
            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("id_contrato"),
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    IdInquilino = reader.GetInt32("id_inquilino"),
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32("anulado_por"),
                    FechaInicio = reader.GetDateTime("f_inicio"),
                    FechaFin = reader.GetDateTime("f_fin"),
                    MontoMensual = reader.GetDouble("monto_mensual"),
                    Estado = reader.GetString("estado"),
                    NyAInquilino = reader.GetString("NyA_inquilino"),
                    Direccion = reader.GetString("direccion"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada")
                });
            }

            return contratos;
        }


        /*public List<Contrato> GetContratosPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            var contratos = new List<Contrato>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT c.*, CONCAT(i.nombre, ' ', i.apellido) AS NyA_inquilino, im.direccion, im.portada  FROM contrato c JOIN inquilino i ON i.id_inquilino = c.id_inquilino JOIN inmueble im ON c.id_inmueble = im.id_inmueble WHERE c.f_inicio = @FechaInicio AND c.f_fin = @FechaFin AND c.estado = 'vigente';", connection);
            command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
            command.Parameters.AddWithValue("@FechaFin", fechaFin);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("id_contrato"),
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    IdInquilino = reader.GetInt32("id_inquilino"),
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32("anulado_por"),
                    FechaInicio = reader.GetDateTime("f_inicio"),
                    FechaFin = reader.GetDateTime("f_fin"),
                    MontoMensual = reader.GetDouble("monto_mensual"),
                    Estado = reader.GetString("estado"),
                    NyAInquilino = reader.GetString("NyA_inquilino"),
                    Direccion = reader.GetString("direccion"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada")
                });
            }

            return contratos;
        }*/

        // Método para obtener un contrato por ID
        public Contrato? GetById(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT c.*, CONCAT(i.nombre, ' ', i.apellido) AS NyA_inquilino, CONCAT(u.nombre, ' ', u.apellido) AS NyA_user, im.direccion, im.portada FROM contrato c JOIN inquilino i ON i.id_inquilino = c.id_inquilino JOIN inmueble im ON c.id_inmueble = im.id_inmueble JOIN usuario u ON c.creado_por = u.id_usuario WHERE id_contrato = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
               return new Contrato
                {
                    IdContrato = reader.GetInt32(reader.GetOrdinal("id_contrato")),
                    IdInmueble = reader.GetInt32(reader.GetOrdinal("id_inmueble")),
                    IdInquilino = reader.GetInt32(reader.GetOrdinal("id_inquilino")),
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32(reader.GetInt32("creado_por")),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32(reader.GetInt32("anulado_por")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("f_inicio")),
                    FechaFin = reader.GetDateTime(reader.GetOrdinal("f_fin")),
                    FechaInicioAnulacion = reader.IsDBNull(reader.GetOrdinal("f_inicio_anulacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("f_inicio_anulacion")),

                    FechaFinAnulacion = reader.IsDBNull(reader.GetOrdinal("f_fin_anulacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("f_fin_anulacion")),

                    MontoMensual = reader.GetDouble(reader.GetOrdinal("monto_mensual")),
                    Estado = reader.GetString(reader.GetOrdinal("estado")),
                    NyAInquilino = reader.GetString(reader.GetOrdinal("NyA_inquilino")),
                    NyAUser = reader.GetString(reader.GetOrdinal("NyA_user")),
                    Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString(reader.GetOrdinal("portada")),
                    
                };

            }

            return null; // Devuelve null si no se encuentra ningún contrato
        }

       


        // Método para agregar un nuevo contrato
        public void InsertContrato(Contrato contrato)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(
                "INSERT INTO `contrato` (`id_inmueble`, `id_inquilino`, `creado_por`, `anulado_por`, `f_inicio`, `f_fin`, `f_inicio_anulacion`, `f_fin_anulacion`, `monto_mensual`, `estado`)" +
                "VALUES (@IdInmueble, @IdInquilino, @CreadoPor, @AnuladoPor, @FechaInicio, @FechaFin,@FechaInicioAnulacion, @FechaFinAnulacion, @MontoMensual, @Estado)", connection);
            command.Parameters.AddWithValue("@IdInmueble", contrato.IdInmueble);
            command.Parameters.AddWithValue("@IdInquilino", contrato.IdInquilino);
            command.Parameters.AddWithValue("@CreadoPor", contrato.CreadoPor ?? 1); // Valor predeterminado: 1
            command.Parameters.AddWithValue("@AnuladoPor", contrato.AnuladoPor ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@FechaInicioAnulacion", contrato.FechaInicioAnulacion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@FechaFinAnulacion", contrato.FechaFinAnulacion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
            command.Parameters.AddWithValue("@Estado", contrato.Estado ?? "Vigente"); // Estado predeterminado: "Vigente"
            command.ExecuteNonQuery();
        }
        
        public void AnularContrato(Contrato contrato)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(
                "UPDATE contrato SET estado = @Estado, anulado_por = @AnuladoPor, f_inicio_anulacion = @FechaInicioAnulacion, f_fin_anulacion = @FechaFinAnulacion WHERE id_contrato = @IdContrato", connection);

            command.Parameters.AddWithValue("@Estado", contrato.Estado);
            command.Parameters.AddWithValue("@AnuladoPor", contrato.AnuladoPor);
            command.Parameters.AddWithValue("@FechaInicioAnulacion", contrato.FechaInicioAnulacion);
            command.Parameters.AddWithValue("@FechaFinAnulacion", contrato.FechaFinAnulacion);
            command.Parameters.AddWithValue("@IdContrato", contrato.IdContrato);

            command.ExecuteNonQuery();
        }

        public void ActualizarContratosPendientes()
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(
                @"UPDATE contrato 
                SET estado = 'anulado' 
                WHERE estado = 'pendiente_anulacion' 
                AND f_fin_anulacion <= NOW()", connection);

            command.ExecuteNonQuery();
        }


        public void FinalizarContratos()
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(
                @"UPDATE contrato 
                SET estado = 'Finalizado' 
                WHERE estado = 'Vigente' 
                AND f_fin < NOW()", connection);

            command.ExecuteNonQuery();
        }

        
        // metodos para los filtraados
        
        public List<Contrato> GetContratosPorInquilino(int idInquilino)
        {
            var contratos = new List<Contrato>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"SELECT c.*, im.direccion, im.portada FROM contrato c JOIN inmueble im ON c.id_inmueble = im.id_inmueble WHERE c.id_inquilino = @IdInquilino AND c.estado = 'Vigente';", connection);
            command.Parameters.AddWithValue("@IdInquilino", idInquilino);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("id_contrato"),
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    IdInquilino = reader.GetInt32("id_inquilino"),
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32("anulado_por"),
                    FechaInicio = reader.GetDateTime("f_inicio"),
                    FechaFin = reader.GetDateTime("f_fin"),
                    MontoMensual = reader.GetDouble("monto_mensual"),
                    Estado = reader.GetString("estado"),
                    Direccion = reader.GetString("direccion"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada")

                });
            }

            return contratos;
        }

    public List<Contrato> GetContratosPorDiasHastaFinalizar(int dias, int pagina, int cantidadPorPagina, out int totalRegistros)
{
    var contratos = new List<Contrato>();
    totalRegistros = 0;
    var hoy = DateTime.Today;
    var fechaLimite = hoy.AddDays(dias);

    using var connection = _dbConnection.GetConnection();

    // Contar total de registros
    using (var countCmd = new MySqlCommand(@"
        SELECT COUNT(*) 
        FROM contrato c 
        WHERE c.f_fin BETWEEN @Hoy AND @FechaLimite;", connection))
    {
        countCmd.Parameters.AddWithValue("@Hoy", hoy);
        countCmd.Parameters.AddWithValue("@FechaLimite", fechaLimite);
        totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
    }

    int offset = (pagina - 1) * cantidadPorPagina;

    using var command = new MySqlCommand(@"
        SELECT c.*, CONCAT(i.nombre, ' ', i.apellido) AS NyA_inquilino, im.direccion, im.portada 
        FROM contrato c 
        JOIN inquilino i ON i.id_inquilino = c.id_inquilino 
        JOIN inmueble im ON c.id_inmueble = im.id_inmueble 
        WHERE c.f_fin BETWEEN @Hoy AND @FechaLimite 
        LIMIT @limit OFFSET @offset;", connection);

    command.Parameters.AddWithValue("@Hoy", hoy);
    command.Parameters.AddWithValue("@FechaLimite", fechaLimite);
    command.Parameters.AddWithValue("@limit", cantidadPorPagina);
    command.Parameters.AddWithValue("@offset", offset);

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        contratos.Add(new Contrato
        {
            IdContrato = reader.GetInt32("id_contrato"),
            IdInmueble = reader.GetInt32("id_inmueble"),
            IdInquilino = reader.GetInt32("id_inquilino"),
            CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32("creado_por"),
            AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? null : reader.GetInt32("anulado_por"),
            FechaInicio = reader.GetDateTime("f_inicio"),
            FechaFin = reader.GetDateTime("f_fin"),
            MontoMensual = reader.GetDouble("monto_mensual"),
            Estado = reader.GetString("estado"),
            NyAInquilino = reader.GetString("NyA_inquilino"),
            Direccion = reader.GetString("direccion"),
            Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada")
        });
    }

    return contratos;
}



    }
}