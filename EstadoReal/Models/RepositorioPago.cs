using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Pagos (Monto, Pagado, Fecha, NroPago, EstadoPago, IdContrato) " +
                    $"VALUES ({p.Monto}, {p.Pagado}, '{p.Fecha}', {p.NroPago}, 1,  {p.IdContrato}) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPago = Convert.ToInt32(id);
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
                string sql = $"UPDATE Pagos SET EstadoPago = 0 WHERE IdPago = {id} ;";
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

        public int Modificacion(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Pagos SET Monto={p.Monto} , Pagado={p.Pagado}, Fecha='{p.Fecha}', NroPago={p.NroPago}, IdContrato={p.IdContrato} " +
                    $"WHERE IdPago = {p.IdPago} ;";
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

        public Pago ObtenerPorId(int id)
        {
            Pago p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago, Monto, Pagado, Fecha, NroPago, EstadoPago, IdContrato " +
                    $" FROM Pagos WHERE IdPago=@id AND EstadoPago = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            Monto = reader.GetDecimal(1),
                            Pagado = reader.GetByte(2),
                            Fecha = reader.GetDateTime(3).ToString(),
                            NroPago = reader.GetInt32(4),
                            EstadoPago = reader.GetByte(5),
                            IdContrato = reader.GetInt32(6),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Pago ObtenerPorIdContrato(int id)
        {
            Pago p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago, Monto, Pagado, Fecha, NroPago, EstadoPago, IdContrato " +
                    $" FROM Pagos WHERE IdContrato=@id AND EstadoPago = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            Monto = reader.GetDecimal(1),
                            Pagado = reader.GetByte(2),
                            Fecha = reader.GetDateTime(3).ToString(),
                            NroPago = reader.GetInt32(4),
                            EstadoPago = reader.GetByte(5),
                            IdContrato = reader.GetInt32(6),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Pago> ObtenerTodos()
        {
            IList<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago, Monto, Pagado, Fecha, NroPago, EstadoPago, IdContrato  " +
                    $" FROM Pagos WHERE EstadoPago = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            Monto = reader.GetDecimal(1),
                            Pagado = reader.GetByte(2),
                            Fecha = reader.GetDateTime(3).ToString(),
                            NroPago = reader.GetInt32(4),
                            EstadoPago = reader.GetByte(5),
                            IdContrato = reader.GetInt32(6),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Pago> ObtenerPagosPorContrato(int id)
        {
            List<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT p.Monto, p.Pagado, p.Fecha, p.NroPago, p.IdPago " +
                    $" FROM Pagos p INNER JOIN Contratos c ON p.IdContrato = c.IdContrato" +
                    $" WHERE p.IdContrato=@id AND p.EstadoPago = 1 ; ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Monto = reader.GetDecimal(0),
                            Pagado = reader.GetByte(1),
                            Fecha = reader.GetDateTime(2).ToString(),
                            NroPago = reader.GetInt32(3),
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
