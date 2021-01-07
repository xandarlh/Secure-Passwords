using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Secure_passwords
{
    public class Database
    {
        private string GetSqlQueryResult(string query, string parameter)
        {
            string queryResult = "";
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocaldb"))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    queryResult = reader[parameter].ToString();
                }

                return queryResult;
            }
        }
        public string GetSaltFromUser(string username)
        {
            var salt = GetSqlQueryResult(
                $@"SELECT TOP (1)[Salt] FROM[LoginDB].[dbo].[Login] WHERE Username = '{username}'", "Salt");
            return salt;
        }
        public void CreateAccountInDb(string username, string salt, string hashedPassword)
        {
            string cmdString = "INSERT INTO LoginDB.dbo.Login (Username,Password,Salt) VALUES (@val1, @val2, @val3)";
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocaldb"))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    comm.Parameters.AddWithValue("@val1", username);
                    comm.Parameters.AddWithValue("@val2", hashedPassword);
                    comm.Parameters.AddWithValue("@val3", salt);
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}
