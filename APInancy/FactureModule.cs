using MySql.Data.MySqlClient;
using Nancy;
using Nancy.ModelBinding;
using System.Configuration;

namespace APInancy
{
    public class FactureModule : NancyModule
    {
        public FactureModule()
        {
            MySqlConnection conn = DBconn.GetConnection();

            //récuperer les factures
            Get("/getfactures", _ =>
            {
                var query = "SELECT * FROM factures";
                var invoices = new List<string>();

                using (var command = new MySqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var factureId = reader.GetInt32(reader.GetOrdinal("facture_id"));
                            var clientId = reader.IsDBNull(reader.GetOrdinal("client_id")) ? -1 : reader.GetInt32(reader.GetOrdinal("client_id"));
                            var factureDate = reader.GetString(reader.GetOrdinal("facture_date"));
                            var description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description"));
                            var total = reader.GetDecimal(reader.GetOrdinal("total"));

                            invoices.Add($"Facture ID: {factureId}, Client ID: {clientId}, Date: {factureDate}, Description: {description}, Total: {total}");
                        }
                    }
                }

                var response = string.Join("<br>", invoices);
                return response;
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
