using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inmuebles (Direccion, UsoResidencial, Tipo, Ambientes, Precio, Disponibilidad, " +
                    $"EstadoInmueble, IdPropietario, Latitud, Longitud) " +
                    $"VALUES ('{i.Direccion}', {i.UsoResidencial}, '{i.Tipo}', {i.Ambientes}, {i.Precio}, " +
                    $"{i.Disponibilidad}, 1 , {i.IdPropietario}, {i.Latitud} , {i.Longitud}) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    i.IdInmueble = Convert.ToInt32(id);
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
                string sql = $"UPDATE Inmuebles SET EstadoInmueble = 0 WHERE IdInmueble = {id}";
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

        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmuebles SET Direccion='{i.Direccion}', UsoResidencial={i.UsoResidencial}, Tipo='{i.Tipo}', Ambientes={i.Ambientes}, Precio= {i.Precio}, " +
                    $"Disponibilidad={i.Disponibilidad}, " +
                    $" EstadoInmueble = 1, IdPropietario = {i.IdPropietario}, Latitud = {i.Latitud}, Longitud = {i.Longitud} WHERE IdInmueble = {i.IdInmueble} ;";
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

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Disponibilidad, EstadoInmueble, IdPropietario, Latitud, Longitud " +
                    $" FROM Inmuebles WHERE IdInmueble=@id AND EstadoInmueble = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            UsoResidencial = reader.GetByte(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponibilidad = reader.GetByte(6),
                            EstadoInmueble = reader.GetByte(7),
                            IdPropietario = reader.GetInt32(8),
                            Latitud = reader.GetDecimal(9),
                            Longitud = reader.GetDecimal(10),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<Inmueble> ObtenerPorIdPropietario(int id)
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, " +
                    $"Precio, Disponibilidad, EstadoInmueble, i.IdPropietario, Latitud, Longitud, p.Nombre, p.Apellido " +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario WHERE i.IdPropietario=@id AND EstadoInmueble = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            UsoResidencial = reader.GetByte(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponibilidad = reader.GetByte(6),
                            EstadoInmueble = reader.GetByte(7),
                            IdPropietario = reader.GetInt32(8),
                            Latitud = reader.GetDecimal(9),
                            Longitud = reader.GetDecimal(10),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(11),
                                Apellido = reader.GetString(12),
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerPorNombrePropietario(string nombre, string apellido)
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, " +
                    $"Precio, Disponibilidad, EstadoInmueble, i.IdPropietario, Latitud, Longitud, p.Nombre, p.Apellido " +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario " +
                    $"WHERE p.Nombre = @nombre AND p.Apellido = @apellido AND EstadoInmueble = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = apellido;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            UsoResidencial = reader.GetByte(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponibilidad = reader.GetByte(6),
                            EstadoInmueble = reader.GetByte(7),
                            IdPropietario = reader.GetInt32(8),
                            Latitud = reader.GetDecimal(9),
                            Longitud = reader.GetDecimal(10),
                            Duenio = new Propietario
                            {
                                Nombre = reader.GetString(11),
                                Apellido = reader.GetString(12),
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerDisponiblesPorIdPropietario(int id)
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Disponibilidad, EstadoInmueble, IdPropietario, Latitud, Longitud " +
                    $" FROM Inmuebles WHERE IdPropietario=@id AND EstadoInmueble = 1 AND Disponibilidad = 1";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            UsoResidencial = reader.GetByte(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponibilidad = reader.GetByte(6),
                            EstadoInmueble = reader.GetByte(7),
                            IdPropietario = reader.GetInt32(8),
                            Latitud = reader.GetDecimal(9),
                            Longitud = reader.GetDecimal(10),
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Disponibilidad, EstadoInmueble, IdPropietario, Latitud, Longitud " +
                    $" FROM Inmuebles WHERE EstadoInmueble = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            UsoResidencial = reader.GetByte(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponibilidad = reader.GetByte(6),
                            EstadoInmueble = reader.GetByte(7),
                            IdPropietario = reader.GetInt32(8),
                            Latitud = reader.GetDecimal(9),
                            Longitud = reader.GetDecimal(10),
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public IList<Inmueble> ObtenerDisponibles()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Disponibilidad, EstadoInmueble, IdPropietario, Latitud, Longitud " +
                    $" FROM Inmuebles WHERE Disponibilidad = 1 AND EstadoInmueble = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            UsoResidencial = reader.GetByte(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponibilidad = reader.GetByte(6),
                            EstadoInmueble = reader.GetByte(7),
                            IdPropietario = reader.GetInt32(8),
                            Latitud = reader.GetDecimal(9),
                            Longitud = reader.GetDecimal(10),
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contrato> ObtenerContratos(int id)
        {
            List<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.InicioContrato, c.FinContrato, c.Deudas, c.IdInquilino, i.Nombre, i.Apellido " +
                    $" FROM Contratos c INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino" +
                    $" WHERE c.IdInmueble=@id AND c.EstadoContrato = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            InicioContrato = reader.GetDateTime(0),
                            FinContrato = reader.GetDateTime(1),
                            Deudas = reader.GetDecimal(2),
                            IdInquilino = reader.GetInt32(3),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(4),
                                Apellido = reader.GetString(5),
                            }
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int ActualizarDisponibilidad(int id, int disponibilidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmuebles SET Disponibilidad=@disponibilidad " +
                    $" WHERE IdInmueble = @id ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id; 
                    command.Parameters.Add("@disponibilidad", SqlDbType.TinyInt).Value = disponibilidad;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

    }
}
