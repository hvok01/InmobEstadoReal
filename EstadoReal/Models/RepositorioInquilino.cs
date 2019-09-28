using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
    {
        public RepositorioInquilino(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Inquilino i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inquilinos (Nombre, Apellido, Dni, LugarTrabajo, Correo, Telefono, NombreGarante, DniGarante, EstadoInquilino) " +
                    $"VALUES ('{i.Nombre}', '{i.Apellido}', {i.Dni}, '{i.LugarTrabajo}', '{i.Correo}', {i.Telefono},'{i.NombreGarante}',{i.DniGarante}, 1) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    i.IdInquilino = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inquilinos SET EstadoInquilino = 0 WHERE IdInquilino = {id} ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inquilino i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inquilinos SET Nombre='{i.Nombre}', Apellido='{i.Apellido}', Dni={i.Dni}, LugarTrabajo='{i.LugarTrabajo}', Correo='{i.Correo}', " +
                    $"Telefono={i.Telefono}, NombreGarante='{i.NombreGarante}', DniGarante={i.DniGarante}, EstadoInquilino = 1 WHERE IdInquilino = {i.IdInquilino} ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inquilino ObtenerPorCorreo(string correo)
        {
            Inquilino i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Nombre, Apellido, Dni, LugarTrabajo, Correo, Telefono, NombreGarante, DniGarante, EstadoInquilino FROM Inquilinos" +
                    $" WHERE Correo=@correo AND EstadoInquilino = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            LugarTrabajo = reader.GetString(4),
                            Correo = reader.GetString(5),
                            Telefono = reader.GetInt64(6),
                            NombreGarante = reader.GetString(7),
                            DniGarante = reader.GetInt32(8),
                            EstadoInquilino = reader.GetByte(9),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Nombre, Apellido, Dni, LugarTrabajo, Correo, Telefono, NombreGarante, DniGarante, EstadoInquilino " +
                    $" FROM Inquilinos WHERE IdInquilino=@id AND EstadoInquilino = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            LugarTrabajo = reader.GetString(4),
                            Correo = reader.GetString(5),
                            Telefono = reader.GetInt64(6),
                            NombreGarante = reader.GetString(7),
                            DniGarante = reader.GetInt32(8),
                            EstadoInquilino = reader.GetByte(9),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Nombre, Apellido, Dni, LugarTrabajo, Correo, Telefono, NombreGarante, DniGarante, EstadoInquilino " +
                    $" FROM Inquilinos WHERE EstadoInquilino = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inquilino i = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            LugarTrabajo = reader.GetString(4),
                            Correo = reader.GetString(5),
                            Telefono = reader.GetInt64(6),
                            NombreGarante = reader.GetString(7),
                            DniGarante = reader.GetInt32(8),
                            EstadoInquilino = reader.GetByte(9),
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
