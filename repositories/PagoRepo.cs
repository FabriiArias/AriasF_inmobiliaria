using MySql.Data.MySqlClient;
using InmobiliariaApp.Data;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Repositories
{
    public class PagoRepo
    {
        private readonly DatabaseConnection _dbConnection;

        public PagoRepo(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Traer todos los pagos 
        public List<Pago> GetAllPagos()
        {
            var pagos = new List<Pago>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                SELECT p.*, CONCAT(u.nombre, ' ', u.apellido) AS nya_usuario 
                FROM pago p 
                JOIN usuario u ON p.creado_por = u.id_usuario", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pagos.Add(new Pago
                {
                    IdPago = reader.GetInt32("id_pago"),
                    IdContrato = reader.GetInt32("id_contrato"),
                    FechaPago = reader["fecha_pago"] is DateTime fecha ? fecha : (DateTime?)null,
                    Importe = reader.GetDouble("importe"),
                    Detalle = reader.GetString("detalle"),
                    Estado = reader.GetString("estado"),
                    CreadoPor = reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? (int?)null : reader.GetInt32("anulado_por"),
                    NyAUser = reader.GetString("nya_usuario")
                });
            }

            return pagos;
        }

        // get all paginados

        public List<Pago> GetAllPagosPaginados(int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var pagos = new List<Pago>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            using (var countCmd = new MySqlCommand(@"SELECT COUNT(*) FROM pago", connection))
            {
                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina -1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT p.*, CONCAT(u.nombre, ' ', u.apellido) AS nya_usuario 
                FROM pago p 
                JOIN usuario u ON p.creado_por = u.id_usuario 
                ORDER BY p.fecha_pago DESC 
                LIMIT @limit OFFSET @offset", connection);

            
            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pagos.Add(new Pago
                {
                    IdPago = reader.GetInt32("id_pago"),
                    IdContrato = reader.GetInt32("id_contrato"),
                    FechaPago = reader["fecha_pago"] is DateTime fecha ? fecha : (DateTime?)null,
                    Importe = reader.GetDouble("importe"),
                    Detalle = reader.GetString("detalle"),
                    Estado = reader.GetString("estado"),
                    CreadoPor = reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? (int?)null : reader.GetInt32("anulado_por"),
                    NyAUser = reader.GetString("nya_usuario")
                });
            }

            return pagos;
        }

        // Traer todos los pagos de un contrato 

        public List<Pago> GetPagosByContrato(int idContrato)
        {
            var pagos = new List<Pago>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"SELECT p.*, CONCAT(u.nombre, ' ', u.apellido) AS nya_usuario, CONCAT(i.nombre, ' ', i.apellido) AS nya_inquilino FROM pago p JOIN usuario u ON p.creado_por = u.id_usuario JOIN contrato c ON p.id_contrato = c.id_contrato JOIN inquilino i ON c.id_inquilino = i.id_inquilino WHERE p.estado = 'Pagado' AND p.id_contrato = @id_contrato", connection);
            command.Parameters.AddWithValue("@id_contrato", idContrato);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pagos.Add(new Pago
                {
                    IdPago = reader.GetInt32("id_pago"),
                    IdContrato = reader.GetInt32("id_contrato"),
                    FechaPago = reader["fecha_pago"] is DateTime fecha ? fecha : (DateTime?)null,
                    Importe = reader.GetDouble("importe"),
                    Detalle = reader.GetString("detalle"),
                    Estado = reader.GetString("estado"),
                    CreadoPor = reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? (int?)null : reader.GetInt32("anulado_por"),
                    NyAUser = reader.GetString("nya_usuario"),
                    NyAInquilino = reader.GetString("nya_inquilino")
                });
            }

            return pagos;
        }

        public List<Pago> GetPagosByContratoPaginado(int idContrato, int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var pagos = new List<Pago>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            using (var countCmd = new MySqlCommand(@"SELECT COUNT(*) FROM pago WHERE estado = 'Pagado' AND id_contrato = @id_contrato", connection))
            {
                countCmd.Parameters.AddWithValue("@id_contrato", idContrato);
                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina - 1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT p.*, CONCAT(u.nombre, ' ', u.apellido) AS nya_usuario, CONCAT(i.nombre, ' ', i.apellido) AS nya_inquilino
                FROM pago p 
                JOIN usuario u ON p.creado_por = u.id_usuario 
                JOIN contrato c ON p.id_contrato = c.id_contrato 
                JOIN inquilino i ON c.id_inquilino = i.id_inquilino 
                WHERE p.estado = 'Pagado' AND p.id_contrato = @id_contrato
                ORDER BY p.fecha_pago DESC
                LIMIT @limit OFFSET @offset", connection);

            command.Parameters.AddWithValue("@id_contrato", idContrato);
            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pagos.Add(new Pago
                {
                    IdPago = reader.GetInt32("id_pago"),
                    IdContrato = reader.GetInt32("id_contrato"),
                    FechaPago = reader["fecha_pago"] is DateTime fecha ? fecha : (DateTime?)null,
                    Importe = reader.GetDouble("importe"),
                    Detalle = reader.GetString("detalle"),
                    Estado = reader.GetString("estado"),
                    CreadoPor = reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? (int?)null : reader.GetInt32("anulado_por"),
                    NyAUser = reader.GetString("nya_usuario"),
                    NyAInquilino = reader.GetString("nya_inquilino")
                });
            }

            return pagos;
        }


        // Obtener un pago por su ID
        public Pago? GetPagoById(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                SELECT p.*, 
                       CONCAT(u.nombre, ' ', u.apellido) AS nya_usuario,
                       CONCAT(i.nombre, ' ', i.apellido) AS nya_inquilino 
                FROM pago p 
                JOIN usuario u ON p.creado_por = u.id_usuario 
                JOIN contrato c ON p.id_contrato = c.id_contrato 
                JOIN inquilino i ON c.id_inquilino = i.id_inquilino 
                WHERE p.id_pago = @id_pago", connection);
            command.Parameters.AddWithValue("@id_pago", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Pago
                {
                    IdPago = reader.GetInt32("id_pago"),
                    IdContrato = reader.GetInt32("id_contrato"),
                    FechaPago = reader["fecha_pago"] is DateTime fecha ? fecha : (DateTime?)null,
                    Importe = reader.GetDouble("importe"),
                    Detalle = reader.GetString("detalle"),
                    Estado = reader.GetString("estado"),
                    CreadoPor = reader.GetInt32("creado_por"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("anulado_por")) ? (int?)null : reader.GetInt32("anulado_por"),
                    NyAUser = reader.GetString("nya_usuario"),
                    NyAInquilino = reader.GetString("nya_inquilino")
                };
            }

            return null;
        }

        // Editar solo el detalle del pago
        public void EditarPago(string detalle, int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                UPDATE pago 
                SET detalle = @detalle 
                WHERE id_pago = @id_pago", connection);
            command.Parameters.AddWithValue("@detalle", detalle);
            command.Parameters.AddWithValue("@id_pago", id);
            command.ExecuteNonQuery();
        }

        // Abonar un pago (actualiza estado, detalle y fecha)
        public void Abonar(Pago pago)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                UPDATE pago 
                SET detalle = @detalle, 
                    estado = @estado, 
                    fecha_pago = @fecha_pago 
                WHERE id_pago = @id_pago", connection);

            command.Parameters.AddWithValue("@detalle", pago.Detalle ?? "");
            command.Parameters.AddWithValue("@estado", pago.Estado);
            command.Parameters.AddWithValue("@fecha_pago", (pago.FechaPago ?? DateTime.Now).ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@id_pago", pago.IdPago);

            command.ExecuteNonQuery();
        }

        public void Anular(Pago pago)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                UPDATE pago 
                SET estado = @estado, 
                    anulado_por = 1
                WHERE id_pago = @id_pago", connection);

            command.Parameters.AddWithValue("@estado", pago.Estado);
            //command.Parameters.AddWithValue("@anulado_por", pago.AnuladoPor ?? 0);
            command.Parameters.AddWithValue("@id_pago", pago.IdPago);

            command.ExecuteNonQuery();
        }

        public void InsertPago(Pago pago)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                INSERT INTO pago (id_contrato, fecha_pago, importe, detalle, estado, creado_por) 
                VALUES (@id_contrato, @fecha_pago, @importe, @detalle, @estado, @creado_por)", connection);

            command.Parameters.AddWithValue("@id_contrato", pago.IdContrato);
            command.Parameters.AddWithValue("@fecha_pago", pago.FechaPago.HasValue ? pago.FechaPago.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("@importe", pago.Importe);
            command.Parameters.AddWithValue("@detalle", pago.Detalle ?? "");
            command.Parameters.AddWithValue("@estado", pago.Estado ?? "Pendiente");
            command.Parameters.AddWithValue("@creado_por", 1); // reemplazar dps por el usuario logueado

            command.ExecuteNonQuery();
        }

        public void InsertMulta(Pago pago)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
            INSERT INTO pago (id_contrato, importe, detalle, estado)", connection);
            command.Parameters.AddWithValue("@id_contrato", pago.IdContrato);
            command.Parameters.AddWithValue("@importe", pago.Importe);
            command.Parameters.AddWithValue("@detalle", pago.Detalle ?? "");
            command.Parameters.AddWithValue("@estado", pago.Estado ?? "Pendiente");

            command.ExecuteNonQuery();

        }

        
    }


    
}
