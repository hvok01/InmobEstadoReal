using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contratos (InicioContrato, FinContrato, Deudas, IdInquilino, IdInmueble) " +
                    $"VALUES ('{c.InicioContrato}', '{c.FinContrato}',{c.Deudas},{c.IdInquilino},{c.IdInmueble})";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    c.IdContrato = Convert.ToInt32(id);
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
                string sql = $"DELETE FROM Contratos WHERE IdContrato = {id}";
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

        public int Modificacion(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contratos SET InicioContrato='{c.InicioContrato}', FinContrato='{c.FinContrato}', Deudas={c.Deudas}, IdInquilino={c.IdInquilino}, IdInmueble={c.IdInmueble} " +
                    $"WHERE IdContrato = {c.IdContrato}";
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

        public Contrato ObtenerPorId(int id)
        {
            Contrato c = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE IdContrato=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            InicioContrato = reader.GetDateTime(1).ToString(),
                            FinContrato = reader.GetDateTime(2).ToString(),
                            Deudas = reader.GetDecimal(3),
                            IdInquilino = reader.GetInt32(4),
                            IdInmueble = reader.GetInt32(5),
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public Contrato ObtenerPorIdInmueble(int id)
        {
            Contrato c = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE IdInmueble=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            InicioContrato = reader.GetDateTime(1).ToString(),
                            FinContrato = reader.GetDateTime(2).ToString(),
                            Deudas = reader.GetDecimal(3),
                            IdInquilino = reader.GetInt32(4),
                            IdInmueble = reader.GetInt32(5),
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public Contrato ObtenerPorIdInquilino(int id)
        {
            Contrato c = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE IdInquilino=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            InicioContrato = reader.GetDateTime(1).ToString(),
                            FinContrato = reader.GetDateTime(2).ToString(),
                            Deudas = reader.GetDecimal(3),
                            IdInquilino = reader.GetInt32(4),
                            IdInmueble = reader.GetInt32(5),
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, IdInquilino, IdInmueble " +
                    $" FROM Contratos";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            InicioContrato = reader.GetDateTime(1).ToString(),
                            FinContrato = reader.GetDateTime(2).ToString(),
                            Deudas = reader.GetDecimal(3),
                            IdInquilino = reader.GetInt32(4),
                            IdInmueble = reader.GetInt32(5),
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
