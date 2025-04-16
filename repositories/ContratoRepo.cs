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
            using var command = new MySqlCommand("SELECT * FROM Contrato", connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("Id"),
                    IdInmueble = reader.GetInt32("DNIInquilino"),
                    IdInquilino = reader.GetInt32("IdInmueble"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    MontoMensual = reader.GetDouble("MontoMensual"),
                    Multa = reader.GetDouble("Multa"),
                    CreadoPor = reader.GetString("CreadoPor"),
                    AnuladoPor = reader.GetString("AnuladoPor"),
                    Activo = reader.GetBoolean("Activo")
                });
            }

            return contratos;
        }

        // Método para obtener un contrato por ID
        public Contrato? GetById(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("SELECT * FROM Contrato WHERE id_contrato = @Id and activo = 1", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Contrato
                {
                    IdContrato = reader.GetInt32("Id"),
                    DNIInquilino = reader.GetInt32("DNIInquilino"),
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    MontoMensual = reader.GetDouble("MontoMensual"),
                    Multa = reader.GetDouble("Multa"),
                    CreadoPor = reader.GetString("CreadoPor"),
                    AnuladoPor = reader.GetString("AnuladoPor"),
                    Activo = reader.GetBoolean("Activo")
                };
            }

            return null; // Devuelve null si no se encuentra ningún contrato
        }

        // Método para agregar un nuevo contrato
        public void InsertContrato(Contrato contrato)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(
                "INSERT INTO `contrato` (`id_inmueble`, `id_inquilino`, `f_inicio`, `f_fin`, `monto_mensual`, `multa`, `creado_por`, `anulado_por`, `activo`)" +
                "VALUES (@IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual, @Multa, @CreadoPor, @AnuladoPor, 1)", connection);

            command.Parameters.AddWithValue("@IdInmueble", contrato.IdInmueble);
            command.Parameters.AddWithValue("@IdInquilino", contrato.DNIInquilino);
            command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
            command.Parameters.AddWithValue("@Multa", contrato.Multa);
            command.Parameters.AddWithValue("@CreadoPor", contrato.CreadoPor);
            command.Parameters.AddWithValue("@AnuladoPor", contrato.AnuladoPor);

            command.ExecuteNonQuery();
        }

        // Método para actualizar un contrato

        //-------------------------------------------- CREAR EL METODO DE NUEVO ---------------------------------
        public void UpdateContrato(Contrato contrato)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand(
                "UPDATE Contrato SET id_inquilino = @InquilinoId, id_inmueble = @InmuebleId, fecha_inicio = @FechaInicio, " +
                "fecha_fin = @FechaFin, monto_mensual = @MontoMensual, estado = @Vigente WHERE id_contato = @Id", connection);

            command.Parameters.AddWithValue("@Id", contrato.IdContrato);
            command.Parameters.AddWithValue("@InquilinoId", contrato.DNIInquilino);
            command.Parameters.AddWithValue("@InmuebleId", contrato.IdInmueble);
            command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
            command.Parameters.AddWithValue("@Vigente", contrato.Activo);

            command.ExecuteNonQuery();
        }

        // Método para eliminar un contrato
        public void Delete(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var command = new MySqlCommand("DELETE FROM Contrato WHERE id_contrato = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }
    }
}