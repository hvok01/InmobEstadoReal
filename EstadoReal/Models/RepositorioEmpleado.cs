using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class RepositorioEmpleado : RepositorioBase, IRepositorioEmpleado
    {
        public RepositorioEmpleado(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Empleado e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Empleados (Nombre, Apellido, Dni, Correo, Clave, EstadoEmpleado) " +
                    $"VALUES ('{e.Nombre}', '{e.Apellido}', {e.Dni}, '{e.Correo}', '{e.Clave}', 1) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    e.IdEmpleado = Convert.ToInt32(id);
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
                string sql = $"UPDATE Empleados SET EstadoEmpleado = 0 WHERE IdEmpleado = {id} ;";
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

        public int Modificacion(Empleado e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Empleados SET Nombre='{e.Nombre}', Apellido='{e.Apellido}', Dni={e.Dni}, Correo='{e.Correo}', " +
                    $"Clave='{e.Clave}', EstadoEmpleado = 1 WHERE IdEmpleado = {e.IdEmpleado} ;";
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

        public Empleado ObtenerPorCorreo(string correo)
        {
            Empleado e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Dni, Correo, Clave, EstadoEmpleado FROM Empleados" +
                    $" WHERE Correo=@correo AND EstadoEmpleado = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Clave = reader.GetString(5),
                            EstadoEmpleado = reader.GetByte(6),
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public Empleado ObtenerPorId(int id)
        {
            Empleado e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Dni, Correo, Clave, EstadoEmpleado FROM Empleados" +
                    $" WHERE IdEmpleado=@id AND EstadoEmpleado = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Clave = reader.GetString(5),
                            EstadoEmpleado = reader.GetByte(6),
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public IList<Empleado> ObtenerTodos()
        {
            List<Empleado> res = new List<Empleado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Dni, Correo, Clave, EstadoEmpleado " +
                    $" FROM Empleados WHERE EstadoEmpleado = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Empleado e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetInt32(3),
                            Correo = reader.GetString(4),
                            Clave = reader.GetString(5),
                            EstadoEmpleado = reader.GetByte(6),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
