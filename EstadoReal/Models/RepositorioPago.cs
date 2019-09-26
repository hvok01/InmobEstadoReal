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
                string sql = $"INSERT INTO Pagos (Monto, Estado, Fecha, NroPago, IdContrato) " +
                    $"VALUES ({p.Monto}, {p.Estado}, '{p.Fecha}', {p.NroPago}, {p.IdContrato}) ;";
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
                string sql = $"DELETE FROM Pagos WHERE IdPago = {id} ;";
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
                string sql = $"UPDATE Pagos SET Monto={p.Monto}, Estado={p.Estado}, Fecha='{p.Fecha}', NroPago={p.NroPago}, IdContrato={p.IdContrato}, " +
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
                string sql = $"SELECT IdPago, Monto, Estado, Fecha, NroPago, IdContrato " +
                    $" FROM Pagos WHERE IdPago=@id ;";
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
                            Estado = reader.GetBoolean(2),
                            Fecha = reader.GetString(3),
                            NroPago = reader.GetInt32(4),
                            IdContrato = reader.GetInt32(5),
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
                string sql = $"SELECT IdPago, Monto, Estado, Fecha, NroPago, IdContrato " +
                    $" FROM Pagos WHERE IdContrato=@id ;";
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
                            Estado = reader.GetBoolean(2),
                            Fecha = reader.GetString(3),
                            NroPago = reader.GetInt32(4),
                            IdContrato = reader.GetInt32(5),
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
                string sql = $"SELECT IdPago, Monto, Estado, Fecha, NroPago, IdContrato " +
                    $" FROM Pagos ;";
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
                            Estado = reader.GetBoolean(2),
                            Fecha = reader.GetString(3),
                            NroPago = reader.GetInt32(4),
                            IdContrato = reader.GetInt32(5),
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
