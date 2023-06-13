using MySql.Data.MySqlClient;
using Nancy;
using Nancy.ModelBinding;
using System.Collections.Generic;

namespace APInancy
{
    public class ClientModule : NancyModule
    {
        public ClientModule()
        {
            MySqlConnection conn = DBconn.GetConnection();

            // Retrieve clients
            Get("/getclients/{param?}", parameters =>
            {
                string param = parameters.param;
                var query = "SELECT * FROM clients";
                var clients = new List<Dictionary<string, object>>();

                if (!string.IsNullOrEmpty(param))
                {
                    // Add WHERE clause to filter by name or id
                    query += " WHERE nom = @Param OR client_id = @Param";
                }

                using (var command = new MySqlCommand(query, conn))
                {
                    // Add parameter to the command
                    command.Parameters.AddWithValue("@Param", param);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new Dictionary<string, object>();

                            client["Nom"] = reader.GetString(reader.GetOrdinal("nom"));
                            client["Email"] = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString(reader.GetOrdinal("email"));
                            client["Telephone"] = reader.IsDBNull(reader.GetOrdinal("telephone")) ? "" : reader.GetString(reader.GetOrdinal("telephone"));
                            client["Adresse"] = reader.IsDBNull(reader.GetOrdinal("adresse")) ? "" : reader.GetString(reader.GetOrdinal("adresse"));
                            client["Code Postal"] = reader.IsDBNull(reader.GetOrdinal("code_postal")) ? 0 : reader.GetInt32(reader.GetOrdinal("code_postal"));

                            clients.Add(client);
                        }
                    }
                }

                return Response.AsJson(clients);
            });

            // Add a client
            Post("/postclient", parameters =>
            {
                ClientPostData postData = this.Bind<ClientPostData>();

                string query = "INSERT INTO clients (nom, email, telephone, adresse, code_postal) " +
                                "VALUES (@Name, @Email, @Phone, @Address, @PostalCode)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Name", postData.Name);
                cmd.Parameters.AddWithValue("@Email", postData.Email);
                cmd.Parameters.AddWithValue("@Phone", postData.Phone);
                cmd.Parameters.AddWithValue("@Address", postData.Address);
                cmd.Parameters.AddWithValue("@PostalCode", postData.PostalCode);

                int rowsAffected = cmd.ExecuteNonQuery();

                return new { success = true, message = $"{rowsAffected} rows affected" };
            });
        }
    }
}
