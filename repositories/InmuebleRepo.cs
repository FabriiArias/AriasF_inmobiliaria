using MySql.Data.MySqlClient;
using InmobiliariaApp.Data;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Repositories
{
    public class InmuebleRepo
    {
        private readonly DatabaseConnection _dbConnection;

        public InmuebleRepo(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        public List<Inmueble> GetAllInmuebles()
        {   
        var inmuebles = new List<Inmueble>();

        using var connection = _dbConnection.GetConnection();
        using var command = new MySqlCommand(@"
            SELECT i.*, p.nombre, p.apellido FROM inmueble i JOIN propietario p ON i.dni_propietario = p.dni_propietario WHERE i.activo = 1 AND p.activo = 1", connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            inmuebles.Add(new Inmueble
            {
                IdInmueble = reader.GetInt32("id_inmueble"),
                DNIPropietario = reader.GetInt32("dni_propietario"),
                Tipo = reader.GetString("tipo"),
                Direccion = reader.GetString("direccion"),
                Ambientes = reader.GetInt32("ambientes"),
                Precio = reader.GetDouble("precio"),
                Estado = reader.GetString("estado"),
                Uso = reader.GetString("uso"),
                Longitud = reader.GetString("longitud"),
                Latitud = reader.GetString("latitud"),
                Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                NombrePropietario = reader.GetString("nombre"),
                ApellidoPropietario = reader.GetString("apellido")
            });
        }

        return inmuebles;
    }

    // paginado de get all 

    public List<Inmueble> GetAllInmueblesPaginados(int pagina, int cantidadPorPagina, out int totalRegistros)
    {
        var inmuebles = new List<Inmueble>();
        totalRegistros = 0;

        using var connection = _dbConnection.GetConnection();

        // obtener el total de registros primero
        using (var countCmd = new MySqlCommand("SELECT COUNT(*) FROM inmueble WHERE activo = 1", connection))
        {
            totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
        }

        int offset = (pagina - 1) * cantidadPorPagina;

        using var command = new MySqlCommand(@"
            SELECT i.*, p.nombre, p.apellido 
            FROM inmueble i 
            JOIN propietario p ON i.dni_propietario = p.dni_propietario 
            WHERE i.activo = 1 
            LIMIT @limit OFFSET @offset", connection);

        command.Parameters.AddWithValue("@limit", cantidadPorPagina);
        command.Parameters.AddWithValue("@offset", offset);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            inmuebles.Add(new Inmueble
            {
                IdInmueble = reader.GetInt32("id_inmueble"),
                DNIPropietario = reader.GetInt32("dni_propietario"),
                Tipo = reader.GetString("tipo"),
                Direccion = reader.GetString("direccion"),
                Ambientes = reader.GetInt32("ambientes"),
                Precio = reader.GetDouble("precio"),
                Estado = reader.GetString("estado"),
                Uso = reader.GetString("uso"),
                Longitud = reader.GetString("longitud"),
                Latitud = reader.GetString("latitud"),
                Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                NombrePropietario = reader.GetString("nombre"),
                ApellidoPropietario = reader.GetString("apellido")
            });
        }

        return inmuebles;
    }

    // ----------------------- filtros ----------------------
    // ------------------ Buscar por propietario ------------------

    public List<Inmueble> BuscarPorPropietario(string nombrePropietario)
    {
        var inmuebles = new List<Inmueble>();

        using var connection = _dbConnection.GetConnection();
        using var command = new MySqlCommand(@"
            SELECT i.*, p.nombre, p.apellido 
            FROM inmueble i 
            JOIN propietario p ON i.dni_propietario = p.dni_propietario 
            WHERE i.activo = 1 AND (p.nombre LIKE @nombre OR p.apellido LIKE @nombre)", connection);

        command.Parameters.AddWithValue("@nombre", "%" + nombrePropietario + "%");

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            inmuebles.Add(new Inmueble
            {
                IdInmueble = reader.GetInt32("id_inmueble"),
                DNIPropietario = reader.GetInt32("dni_propietario"),
                Tipo = reader.GetString("tipo"),
                Direccion = reader.GetString("direccion"),
                Ambientes = reader.GetInt32("ambientes"),
                Precio = reader.GetDouble("precio"),
                Estado = reader.GetString("estado"),
                Uso = reader.GetString("uso"),
                Longitud = reader.GetString("longitud"),
                Latitud = reader.GetString("latitud"),
                Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                NombrePropietario = reader.GetString("nombre"),
                ApellidoPropietario = reader.GetString("apellido")
            });
        }

        return inmuebles;
    }


        // por propietario paginado

        public List<Inmueble> BuscarPorPropietarioPaginado(string nombrePropietario, int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var inmuebles = new List<Inmueble>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            // obtener el total de registros primero
            using (var countCmd = new MySqlCommand(@"
                SELECT COUNT(*) 
                FROM inmueble i 
                JOIN propietario p ON i.dni_propietario = p.dni_propietario 
                WHERE i.activo = 1 AND (p.nombre LIKE @nombre OR p.apellido LIKE @nombre)", connection))
            {
                countCmd.Parameters.AddWithValue("@nombre", "%" + nombrePropietario + "%");
                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina - 1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT i.*, p.nombre, p.apellido 
                FROM inmueble i 
                JOIN propietario p ON i.dni_propietario = p.dni_propietario 
                WHERE i.activo = 1 AND (p.nombre LIKE @nombre OR p.apellido LIKE @nombre) 
                LIMIT @limit OFFSET @offset", connection);

            command.Parameters.AddWithValue("@nombre", "%" + nombrePropietario + "%");
            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inmuebles.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    DNIPropietario = reader.GetInt32("dni_propietario"),
                    Tipo = reader.GetString("tipo"),
                    Direccion = reader.GetString("direccion"),
                    Ambientes = reader.GetInt32("ambientes"),
                    Precio = reader.GetDouble("precio"),
                    Estado = reader.GetString("estado"),
                    Uso = reader.GetString("uso"),
                    Longitud = reader.GetString("longitud"),
                    Latitud = reader.GetString("latitud"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    NombrePropietario = reader.GetString("nombre"),
                    ApellidoPropietario = reader.GetString("apellido")
                });
            }
            return inmuebles;
        }

        // ------------------ Buscar por estado disponible ------------------
        /*
        public List<Inmueble> ObtenerDisponibles()
        {
            var inmuebles = new List<Inmueble>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                SELECT i.*, p.nombre, p.apellido 
                FROM inmueble i 
                JOIN propietario p ON i.dni_propietario = p.dni_propietario 
                WHERE i.activo = 1 AND i.estado = 'disponible'", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inmuebles.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    DNIPropietario = reader.GetInt32("dni_propietario"),
                    Tipo = reader.GetString("tipo"),
                    Direccion = reader.GetString("direccion"),
                    Ambientes = reader.GetInt32("ambientes"),
                    Precio = reader.GetDouble("precio"),
                    Estado = reader.GetString("estado"),
                    Uso = reader.GetString("uso"),
                    Longitud = reader.GetString("longitud"),
                    Latitud = reader.GetString("latitud"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    NombrePropietario = reader.GetString("nombre"),
                    ApellidoPropietario = reader.GetString("apellido")
                });
            }

            return inmuebles;
        }*/

        // por estado disponible paginado

        public List<Inmueble> ObtenerDisponiblesPaginado(int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var inmuebles = new List<Inmueble>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            // obtener el total de registros primero
            using (var countCmd = new MySqlCommand("SELECT COUNT(*) FROM inmueble WHERE activo = 1 AND estado = 'disponible'", connection))
            {
                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina - 1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT i.*, p.nombre, p.apellido 
                FROM inmueble i 
                JOIN propietario p ON i.dni_propietario = p.dni_propietario 
                WHERE i.activo = 1 AND i.estado = 'disponible' 
                LIMIT @limit OFFSET @offset", connection);

            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inmuebles.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    DNIPropietario = reader.GetInt32("dni_propietario"),
                    Tipo = reader.GetString("tipo"),
                    Direccion = reader.GetString("direccion"),
                    Ambientes = reader.GetInt32("ambientes"),
                    Precio = reader.GetDouble("precio"),
                    Estado = reader.GetString("estado"),
                    Uso = reader.GetString("uso"),
                    Longitud = reader.GetString("longitud"),
                    Latitud = reader.GetString("latitud"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    NombrePropietario = reader.GetString("nombre"),
                    ApellidoPropietario = reader.GetString("apellido")
                });
            }

            return inmuebles;
        }

        // ------------------ Buscar por fecha no contratada ------------------

        public List<Inmueble> ObtenerDisponiblesPorFecha(DateTime fecha)
        {
            var inmuebles = new List<Inmueble>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"SELECT DISTINCT i.*, p.nombre, p.apellido FROM inmueble i LEFT JOIN contrato c ON i.id_inmueble = c.id_inmueble JOIN propietario p ON p.DNI_propietario = i.dni_propietario WHERE NOT EXISTS ( SELECT 1 FROM contrato c2 WHERE c2.id_inmueble = i.id_inmueble AND @fecha BETWEEN c2.f_inicio AND c2.f_fin AND c2.estado NOT IN ('Anulado') );", connection);

            command.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy-MM-dd"));

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inmuebles.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    DNIPropietario = reader.GetInt32("dni_propietario"),
                    Tipo = reader.GetString("tipo"),
                    Direccion = reader.GetString("direccion"),
                    Ambientes = reader.GetInt32("ambientes"),
                    Precio = reader.GetDouble("precio"),
                    Estado = reader.GetString("estado"),
                    Uso = reader.GetString("uso"),
                    Longitud = reader.GetString("longitud"),
                    Latitud = reader.GetString("latitud"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    NombrePropietario = reader.GetString("nombre"),
                    ApellidoPropietario = reader.GetString("apellido")
                });
            }

            return inmuebles;
        }

        // por fecha no contratada paginado

        public List<Inmueble> ObtenerDisponiblesPorFechaPaginado(DateTime fecha, int pagina, int cantidadPorPagina, out int totalRegistros)
        {
            var inmuebles = new List<Inmueble>();
            totalRegistros = 0;

            using var connection = _dbConnection.GetConnection();

            // obtener el total de registros primero
            using (var countCmd = new MySqlCommand(@"
                SELECT COUNT(DISTINCT i.id_inmueble) 
                FROM inmueble i 
                LEFT JOIN contrato c ON i.id_inmueble = c.id_inmueble 
                WHERE NOT EXISTS (
                    SELECT 1 FROM contrato c2 
                    WHERE c2.id_inmueble = i.id_inmueble 
                    AND @fecha BETWEEN c2.f_inicio AND c2.f_fin 
                    AND c2.estado NOT IN ('Anulado')
                )", connection))
            {
                countCmd.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy-MM-dd"));
                totalRegistros = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            int offset = (pagina - 1) * cantidadPorPagina;

            using var command = new MySqlCommand(@"
                SELECT DISTINCT i.*, p.nombre, p.apellido 
                FROM inmueble i 
                LEFT JOIN contrato c ON i.id_inmueble = c.id_inmueble 
                JOIN propietario p ON p.DNI_propietario = i.dni_propietario 
                WHERE NOT EXISTS (
                    SELECT 1 FROM contrato c2 
                    WHERE c2.id_inmueble = i.id_inmueble 
                    AND @fecha BETWEEN c2.f_inicio AND c2.f_fin 
                    AND c2.estado NOT IN ('Anulado')
                )
                LIMIT @limit OFFSET @offset", connection);

            command.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@limit", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inmuebles.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("id_inmueble"),
                    DNIPropietario = reader.GetInt32("dni_propietario"),
                    Tipo = reader.GetString("tipo"),
                    Direccion = reader.GetString("direccion"),
                    Ambientes = reader.GetInt32("ambientes"),
                    Precio = reader.GetDouble("precio"),
                    Estado = reader.GetString("estado"),
                    Uso = reader.GetString("uso"),
                    Longitud = reader.GetString("longitud"),
                    Latitud = reader.GetString("latitud"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    NombrePropietario = reader.GetString("nombre"),
                    ApellidoPropietario = reader.GetString("apellido")
                });
            }
            return inmuebles;
        }


        public Inmueble? GetInmuebleXID(int id){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT i.*, p.nombre, p.apellido FROM inmueble i JOIN propietario p ON p.DNI_propietario = i.dni_propietario WHERE Id_Inmueble = @id ", connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Inmueble
                {
                    IdInmueble = reader.GetInt32("Id_Inmueble"),
                    DNIPropietario = reader.GetInt32("DNI_Propietario"),
                    Tipo = reader.GetString("tipo"),
                    Direccion = reader.GetString("direccion"),
                    Ambientes = reader.GetInt32("ambientes"),
                    Precio = reader.GetDouble("precio"),
                    Estado = reader.GetString("estado"),
                    Uso = reader.GetString("uso"),
                    Longitud = reader.GetString("longitud"),
                    Latitud = reader.GetString("latitud"),
                    Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                    NombrePropietario = reader.GetString("nombre"),
                    ApellidoPropietario = reader.GetString("apellido")
                };
            }
            return null;
            
        }

        public void InsertInmueble(Inmueble inmueble){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("INSERT INTO inmueble (DNI_Propietario, tipo, direccion, ambientes, precio, estado, uso, longitud, latitud, portada, activo) VALUES (@DNI, @Tipo, @Direccion, @Ambientes, @Precio, @Estado, @Uso, @Longitud, @Latitud, @Portada, 1)", connection);

            command.Parameters.AddWithValue("@DNI", inmueble.DNIPropietario);
            command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
            command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
            command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
            command.Parameters.AddWithValue("@Precio", inmueble.Precio);
            command.Parameters.AddWithValue("@Estado", inmueble.Estado);
            command.Parameters.AddWithValue("@Uso", inmueble.Uso);
            command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
            command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
            command.Parameters.AddWithValue("@Portada", inmueble.Portada);

            command.ExecuteNonQuery();

        }

        public void UpdateInmueble(Inmueble inmueble){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE inmueble SET DNI_Propietario = @DNI, tipo = @Tipo , direccion = @Direccion, ambientes = @Ambientes, precio = @Precio, estado = @Estado, uso = @Uso, longitud = @Longitud, latitud = @Latitud, portada = @Portada WHERE Id_Inmueble = @Id", connection);

            command.Parameters.AddWithValue("@Id", inmueble.IdInmueble);
            command.Parameters.AddWithValue("@DNI", inmueble.DNIPropietario);
            command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
            command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
            command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
            command.Parameters.AddWithValue("@Precio", inmueble.Precio);
            command.Parameters.AddWithValue("@Estado", inmueble.Estado);
            command.Parameters.AddWithValue("@Uso", inmueble.Uso);
            command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
            command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
            command.Parameters.AddWithValue("@Portada", inmueble.Portada);

            command.ExecuteNonQuery();
            }


            public void DeleteInmueble(int id){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE inmueble SET activo = 0 WHERE Id_Inmueble = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            
            }


            public List<Inmueble> GetInmueblesDisponibles(string tipo, string uso, int ambientes, DateTime fechaInicio, DateTime fechaFin)
            {
                var disponibles = new List<Inmueble>();

                using var connection = _dbConnection.GetConnection();
                using var command = new MySqlCommand(@"
                    SELECT
                        i.*,
                        p.nombre,
                        p.apellido
                    FROM
                        inmueble i
                    JOIN propietario p ON
                        i.dni_propietario = p.dni_propietario
                    LEFT JOIN contrato c ON
                        i.id_inmueble = c.id_inmueble 
                        AND c.f_inicio <= @FechaFin 
                        AND c.f_fin >= @FechaInicio
                    WHERE
                        i.activo = 1 
                        AND i.tipo = @Tipo 
                        AND i.uso = @Uso 
                        AND i.ambientes = @Ambientes
                        AND (c.id_contrato IS NULL OR c.estado NOT IN ('Vigente', 'Pendiente_anular'));
                                    "
                , connection);

                command.Parameters.AddWithValue("@Tipo", tipo);
                command.Parameters.AddWithValue("@Uso", uso);
                command.Parameters.AddWithValue("@Ambientes", ambientes);
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@FechaFin", fechaFin.ToString("yyyy-MM-dd"));

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    disponibles.Add(new Inmueble
                    {
                        IdInmueble = reader.GetInt32("id_inmueble"),
                        DNIPropietario = reader.GetInt32("dni_propietario"),
                        Tipo = reader.GetString("tipo"),
                        Direccion = reader.GetString("direccion"),
                        Ambientes = reader.GetInt32("ambientes"),
                        Precio = reader.GetDouble("precio"),
                        Estado = reader.GetString("estado"),
                        Uso = reader.GetString("uso"),
                        Longitud = reader.GetString("longitud"),
                        Latitud = reader.GetString("latitud"),
                        Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                        NombrePropietario = reader.GetString("nombre"),
                        ApellidoPropietario = reader.GetString("apellido")
                    });
                }

                return disponibles;
            }

        
        public bool EstaDisponible(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(@"
                SELECT COUNT(*) FROM contrato
                WHERE id_inmueble = @id
                AND estado = 'Vigente'
                AND (
                    (@inicio BETWEEN f_inicio AND f_fin)
                    OR (@fin BETWEEN f_inicio AND f_fin)
                    OR (f_inicio BETWEEN @inicio AND @fin)
                )", connection);

            command.Parameters.AddWithValue("@id", idInmueble);
            command.Parameters.AddWithValue("@inicio", fechaInicio);
            command.Parameters.AddWithValue("@fin", fechaFin);

            return Convert.ToInt32(command.ExecuteScalar()) == 0;
        }

            }
}