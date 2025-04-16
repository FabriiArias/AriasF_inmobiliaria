using MySql.Data.MySqlClient;
using InmobiliariaApp.Data;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Repositories
{
    public class PropietarioRepo
    {
        private readonly DatabaseConnection _dbConnection;

        public PropietarioRepo(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        // traer todos los propietarios 
        public List<Propietario> GetAllPropietarios()
        {
            var propietarios = new List<Propietario>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Propietario where activo = 1", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                propietarios.Add(new Propietario
                {
                    DNIPropietario = reader.GetInt32("DNI_Propietario"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Celular = reader.GetInt32("celular"),
                    Email = reader.GetString("mail")
                });
            }

            return propietarios;
        }

        public Propietario? GetPropietarioByDNI(int dni){
            
            Console.WriteLine("DNI repo: " + dni);
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Propietario WHERE DNI_Propietario = @dni AND activo = 1", connection);
            command.Parameters.AddWithValue("@dni", dni);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Propietario
                {
                    DNIPropietario = reader.GetInt32("DNI_Propietario"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Celular = reader.GetInt32("celular"),
                    Email = reader.GetString("mail")
                };
            }
            return null;
        }

        public void InsertPropietario(Propietario propietario){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("INSERT INTO Propietario (DNI_Propietario, nombre, apellido, celular, mail, activo) VALUES (@DNI, @Nombre, @Apellido, @Celular, @Email, 1)", connection);

            command.Parameters.AddWithValue("@DNI", propietario.DNIPropietario);
            command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
            command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
            command.Parameters.AddWithValue("@Celular", propietario.Celular);
            command.Parameters.AddWithValue("@Email", propietario.Email);

            command.ExecuteNonQuery();
        }

        public void UpdatePropietario(Propietario propietario){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE propietario SET DNI_propietario = @DNI, nombre = @Nombre , apellido = @Apellido, celular = @Celular, mail = @Email WHERE DNI_propietario = @DNI", connection);

            command.Parameters.AddWithValue("@DNI", propietario.DNIPropietario);
            command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
            command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
            command.Parameters.AddWithValue("@Celular", propietario.Celular);
            command.Parameters.AddWithValue("@Email", propietario.Email);

            command.ExecuteNonQuery();
            }
        public void deletePropietario(int dni){
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE propietario SET activo = 0 WHERE DNI_propietario = @DNI", connection);
            command.Parameters.AddWithValue("@dni", dni);
            command.ExecuteNonQuery();
            }
    }
}