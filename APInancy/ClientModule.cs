using MySql.Data.MySqlClient;
using Nancy;
using Nancy.ModelBinding;
using System.Configuration;

namespace APInancy
{
    public class ClientModule : NancyModule
    {
        public ClientModule()
        {
            MySqlConnection conn = DBconn.GetConnection();
            
            //Ajoute tout les clients dans une liste puis renvoie la liste. 
            Get("/getclients", _ =>
            {
                {
                    var query = "SELECT * FROM clients";
                    var clients = new List<string>();

                    using (var command = new MySqlCommand(query, conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var name = reader.GetString(reader.GetOrdinal("nom"));
                                var email = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString(reader.GetOrdinal("email"));
                                var phone = reader.IsDBNull(reader.GetOrdinal("telephone")) ? "" : reader.GetString(reader.GetOrdinal("telephone"));
                                var address = reader.IsDBNull(reader.GetOrdinal("adresse")) ? "" : reader.GetString(reader.GetOrdinal("adresse"));

                                clients.Add($"Nom: {name}, Email: {email}, telephone: {phone}, Adresse: {address}");
                            }
                        }
                    }
                    var response = string.Join("<br>", clients);
                    return response;

                }
            });

            //Ajoute un client avec les infos envoyer sur Postman. 
            //Penser a faire une page HTML dédiée quand il le faudra.
            Post("/postclient", parameters =>
            {
                ClientPostData postData = this.Bind<ClientPostData>();

                string query = "INSERT INTO clients (nom, email, telephone, adresse) " +
                               "VALUES (@Name, @Email, @Phone, @Address)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Name", postData.Name);
                cmd.Parameters.AddWithValue("@Email", postData.Email);
                cmd.Parameters.AddWithValue("@Phone", postData.Phone);
                cmd.Parameters.AddWithValue("@Address", postData.Address);

                int rowsAffected = cmd.ExecuteNonQuery();

                return new { success = true, message = $"{rowsAffected} rows affected" };
            });
        }
    }
}
