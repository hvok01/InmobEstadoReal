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
                string sql = $"INSERT INTO Inmuebles (Direccion, UsoResidencial, Tipo, Ambientes, Precio, Estado, IdPropietario) " +
                    $"VALUES ('{i.Direccion}', {i.UsoResidencial},'{i.Tipo}',{i.Ambientes},{i.Precio},{i.Estado},{i.IdPropietario})";
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
                string sql = $"DELETE FROM Inmuebles WHERE IdInmueble = {id}";
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
                string sql = $"UPDATE Inmuebles SET Direccion='{i.Direccion}', UsoResidencial={i.UsoResidencial}, Tipo='{i.Tipo}', Ambientes={i.Ambientes}, Precio={i.Precio}, " +
                    $"Estado={i.Estado}, IdPropietario={i.IdPropietario} WHERE IdInmueble = {i.IdInmueble}";
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
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Estado, IdPropietario " +
                    $" FROM Inmuebles WHERE IdInmueble=@id";
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
                            UsoResidencial = reader.GetBoolean(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public Inmueble ObtenerPorIdPropietario(int id)
        {
            Inmueble i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Estado, IdPropietario " +
                    $" FROM Inmuebles WHERE IdPropietario=@id";
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
                            UsoResidencial = reader.GetBoolean(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, UsoResidencial, Tipo, Ambientes, Precio, Estado, IdPropietario " +
                    $" FROM Inmuebles";
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
                            UsoResidencial = reader.GetBoolean(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
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
