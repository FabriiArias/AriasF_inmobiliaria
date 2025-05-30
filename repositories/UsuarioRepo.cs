using MySql.Data.MySqlClient;
using InmobiliariaApp.Data;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Repositories
{
    public class UsuarioRepo
    {
        private readonly DatabaseConnection _dbConnection;

        public UsuarioRepo(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Usuario> GetAllUsuarios()
        {
            var usuarios = new List<Usuario>();

            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Usuario where activo = 1", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                usuarios.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32("id_usuario"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Rol = reader.GetString("rol"),
                    Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Contacto = reader.GetString("contacto"),
                    Activo = reader.GetBoolean("activo") // Agregado
                });
            }

            return usuarios;
        }

        public Usuario? GetUsuarioById(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Usuario WHERE id_usuario = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32("id_usuario"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Rol = reader.GetString("rol"),
                    Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Contacto = reader.GetString("contacto"),
                    Activo = reader.GetBoolean("activo") // Agregado
                };
            }
            return null;
        }

        public void InsertUsuario(Usuario usuario)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("INSERT INTO Usuario (email, password, rol, avatar, nombre, apellido, contacto, activo) VALUES (@Email, @Password, @Rol, @Avatar, @Nombre, @Apellido, @Contacto, @Activo)", connection);

            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@Password", usuario.Password);
            command.Parameters.AddWithValue("@Rol", usuario.Rol);
            command.Parameters.AddWithValue("@Avatar", usuario.Avatar ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            command.Parameters.AddWithValue("@Contacto", usuario.Contacto);
            command.Parameters.AddWithValue("@Activo", usuario.Activo); // Agregado

            command.ExecuteNonQuery();
        }

        public void UpdateUsuario(Usuario usuario)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE Usuario SET email = @Email, password = @Password, rol = @Rol, avatar = @Avatar, nombre = @Nombre, apellido = @Apellido, contacto = @Contacto, activo = @Activo WHERE id_usuario = @Id", connection);

            command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@Password", usuario.Password);
            command.Parameters.AddWithValue("@Rol", usuario.Rol);
            command.Parameters.AddWithValue("@Avatar", usuario.Avatar ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            command.Parameters.AddWithValue("@Contacto", usuario.Contacto);
            command.Parameters.AddWithValue("@Activo", usuario.Activo); // Agregado

            command.ExecuteNonQuery();
        }

        public Usuario? GetUsuarioByEmail(string email)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Usuario WHERE email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32("id_usuario"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Rol = reader.GetString("rol"),
                    Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
                    Nombre = reader.GetString("nombre"),
                    Apellido = reader.GetString("apellido"),
                    Contacto = reader.GetString("contacto"),
                    Activo = reader.GetBoolean("activo")
                };
            }
            return null;
        }

        public void ActualizarAvatar(int id, string avatar)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE Usuario SET avatar = @Avatar WHERE id_usuario = @Id", connection);
            command.Parameters.AddWithValue("@Avatar", avatar ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }

        public void BorrarAvatar(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE Usuario SET avatar = NULL WHERE id_usuario = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }

        public void DeleteUsuario(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("UPDATE usuario SET activo = 0 WHERE id_usuario = @Id;", connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }


    }
}
