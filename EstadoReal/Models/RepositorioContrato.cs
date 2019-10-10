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
                string sql = $"INSERT INTO Contratos (InicioContrato, FinContrato, Deudas, IdInquilino, IdInmueble, EstadoContrato) " +
                    $"VALUES ('{c.InicioContrato}', '{c.FinContrato}',{c.Deudas},{c.IdInquilino},{c.IdInmueble}, 1) ;";
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
                string sql = $"UPDATE Contratos SET EstadoContrato = 0 WHERE IdContrato = {id}";
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
                string sql = $"UPDATE Contratos SET InicioContrato='{c.InicioContrato}', FinContrato='{c.FinContrato}', Deudas={c.Deudas}, EstadoContrato = 1, " +
                    $"IdInquilino = {c.IdInquilino}, IdInmueble = {c.IdInmueble} WHERE IdContrato = {c.IdContrato}";
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
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, EstadoContrato, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE IdContrato=@id AND EstadoContrato = 1 ;";
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
                            EstadoContrato = reader.GetByte(4),
                            IdInquilino = reader.GetInt32(5),
                            IdInmueble = reader.GetInt32(6),
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
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, EstadoContrato, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE IdInmueble=@id AND EstadoContrato = 1 ;";
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
                            EstadoContrato = reader.GetByte(4),
                            IdInquilino = reader.GetInt32(5),
                            IdInmueble = reader.GetInt32(6),
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
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, EstadoContrato, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE IdInquilino=@id AND EstadoContrato = 1 ;";
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
                            EstadoContrato = reader.GetByte(4),
                            IdInquilino = reader.GetInt32(5),
                            IdInmueble = reader.GetInt32(6),
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
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, EstadoContrato, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE EstadoContrato = 1 ;";
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
                            EstadoContrato = reader.GetByte(4),
                            IdInquilino = reader.GetInt32(5),
                            IdInmueble = reader.GetInt32(6),
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contrato> BuscarEntreFechas(string fechaA, string fechaB)
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, InicioContrato, FinContrato, Deudas, EstadoContrato, IdInquilino, IdInmueble " +
                    $" FROM Contratos WHERE EstadoContrato = 1 AND InicioContrato BETWEEN @fechaA AND @fechaB" +
                    $" AND FinContrato BETWEEN @fechaA AND @fechaB ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fechaA", SqlDbType.Date).Value = fechaA;
                    command.Parameters.Add("@fechaB", SqlDbType.Date).Value = fechaB;
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
                            EstadoContrato = reader.GetByte(4),
                            IdInquilino = reader.GetInt32(5),
                            IdInmueble = reader.GetInt32(6),
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
