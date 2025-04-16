using MySql.Data.MySqlClient;
using InmobiliariaApp.Data;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Repositories
{
    public class InquilinoRepo
    {
        private readonly DatabaseConnection _dbConnection;

        public InquilinoRepo(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // traer todos los inquilinos 
        public List<Inquilino> GetAllInquilinos()
        {
            var inquilinos = new List<Inquilino>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Inquilino where activo = 1", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inquilinos.Add(new Inquilino
                {
                    Id = reader.GetInt32("id_inquilino"),
                    DNIInquilino = reader.GetInt32("DNI_Inquilino"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Celular = reader.GetInt32("celular"),
                    Email = reader.GetString("email")
                });
            }

            return inquilinos;
        }
        public Inquilino? GetInquilinoByDNI(int id)
        {

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Inquilino WHERE id_inquilino = @id AND activo = 1", connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Inquilino
                {
                    Id = reader.GetInt32("id_inquilino"),
                    DNIInquilino = reader.GetInt32("DNI_Inquilino"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Celular = reader.GetInt32("celular"),
                    Email = reader.GetString("email")
                };
            }

            return null;
        }

        public void CrearInquilino(Inquilino inquilino)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("INSERT INTO Inquilino (DNI_Inquilino, nombre, apellido, celular, email, activo) VALUES (@DNI_Inquilino, @nombre, @apellido, @celular, @email, 1)", connection);
            command.Parameters.AddWithValue("@DNI_Inquilino", inquilino.DNIInquilino);
            command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
            command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
            command.Parameters.AddWithValue("@celular", inquilino.Celular);
            command.Parameters.AddWithValue("@email", inquilino.Email);

            command.ExecuteNonQuery();
        }

        public void ActualizarInquilino(Inquilino inquilino)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE inquilino SET dni_inquilino = @DNI, nombre = @nombre, apellido = @apellido, celular = @celular, email = @email WHERE id_inquilino = @Id", connection);
            command.Parameters.AddWithValue("@Id", inquilino.Id);
            command.Parameters.AddWithValue("@DNI", inquilino.DNIInquilino);
            command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
            command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
            command.Parameters.AddWithValue("@celular", inquilino.Celular);
            command.Parameters.AddWithValue("@email", inquilino.Email);

            command.ExecuteNonQuery();
        }

        public void DeleteInquilino(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE inquilino SET activo = 0 WHERE id_inquilino = @Id", connection);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
            }
    }
}