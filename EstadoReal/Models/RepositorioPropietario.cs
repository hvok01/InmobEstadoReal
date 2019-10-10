using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Propietario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Propietarios (Nombre, Apellido, Dni, Correo, Telefono, Clave, EstadoPropietario) " +
                    $"VALUES ('{p.Nombre}', '{p.Apellido}',{p.Dni},'{p.Correo}',{p.Telefono},'{p.Clave}', 1) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPropietario = Convert.ToInt32(id);
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
                string sql = $"UPDATE Propietarios SET EstadoPropietario = 0 WHERE idPropietario = {id}";
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

        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Propietarios SET Nombre='{p.Nombre}', Apellido='{p.Apellido}', Dni={p.Dni}, Correo='{p.Correo}', " +
                    $"Telefono={p.Telefono}, Clave='{p.Clave}', EstadoPropietario = 1 " +
                    $"WHERE IdPropietario = {p.IdPropietario}";
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

        public Propietario ObtenerPorCorreo(string correo)
        {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Correo, Telefono, Clave, EstadoPropietario FROM Propietarios" +
                    $" WHERE Correo=@correo";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Telefono = reader.GetInt64(5),
                            Clave = reader.GetString(6),
                            EstadoPropietario = reader.GetByte(7),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Correo, Telefono, Clave, EstadoPropietario FROM Propietarios" +
                    $" WHERE IdPropietario=@id AND EstadoPropietario = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Telefono = reader.GetInt64(5),
                            Clave = reader.GetString(6),
                            EstadoPropietario = reader.GetByte(7),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Correo, Telefono, Clave, EstadoPropietario" +
                    $" FROM Propietarios WHERE EstadoPropietario = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Telefono = reader.GetInt64(5),
                            Clave = reader.GetString(6),
                            EstadoPropietario = reader.GetByte(7),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Propietario> ObtenerPorNombreApellido(string nombre, string apellido)
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Correo, Telefono, Clave, EstadoPropietario" +
                    $" FROM Propietarios WHERE EstadoPropietario = 1 AND Nombre = @nombre AND Apellido = @apellido ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = apellido;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Telefono = reader.GetInt64(5),
                            Clave = reader.GetString(6),
                            EstadoPropietario = reader.GetByte(7),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;

        }
    }
}
