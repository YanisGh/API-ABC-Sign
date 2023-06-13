using MySql.Data.MySqlClient;
using Nancy;
using Nancy.ModelBinding;
using System.Configuration;
using System.Collections.Generic;

namespace APInancy
{
    public class FactureModule : NancyModule
    {
        public FactureModule()
        {
            MySqlConnection conn = DBconn.GetConnection();

            // Retrieve invoices
            // Retrieve invoices
            Get("/getfactures/{id?}", parameters =>
            {
                string id = parameters.id;
                var query = "SELECT * FROM factures";
                var invoices = new List<Dictionary<string, object>>();

                if (!string.IsNullOrEmpty(id))
                {
                    // Add WHERE clause to filter by facture_id
                    query += " WHERE facture_id = @Id";
                }

                using (var command = new MySqlCommand(query, conn))
                {
                    // Add parameter to the command
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var invoice = new Dictionary<string, object>();

                            invoice["Facture ID"] = reader.GetInt32(reader.GetOrdinal("facture_id"));
                            invoice["Client ID"] = reader.IsDBNull(reader.GetOrdinal("client_id")) ? -1 : reader.GetInt32(reader.GetOrdinal("client_id"));
                            invoice["Date"] = reader.GetString(reader.GetOrdinal("facture_date"));
                            invoice["Description"] = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description"));
                            invoice["Total"] = reader.GetDecimal(reader.GetOrdinal("total"));

                            invoices.Add(invoice);
                        }
                    }
                }
                
                return Response.AsJson(invoices);
            });


            // Add an invoice
            Post("/postfacture", parameters =>
            {
                FacturePostData postData = this.Bind<FacturePostData>();

                string query = "INSERT INTO factures (client_id, facture_date, description, total) " +
                               "VALUES (@Client_id, @FactureDate, @Description, @Total)";
                
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Client_id", postData.Client_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FactureDate", postData.FactureDate);
                cmd.Parameters.AddWithValue("@Description", postData.Description);
                cmd.Parameters.AddWithValue("@Total", postData.Total);

                int rowsAffected = cmd.ExecuteNonQuery();

                return new { success = true, message = $"{rowsAffected} rows affected" };
            });
        }
    }
}
