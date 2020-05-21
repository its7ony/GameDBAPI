using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GameAPI.Service
{
    public class UserService
    {
        public SqlConnection cnn;
        public bool makeConnection()
        {
            string connectionString = null;
            connectionString = "Server=DESKTOP-16G1T07;Database=GameDB;User Id=sa;Password=12345;";
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                cnn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public User getUser(string id, string pwd)
        {
            User user = new User();
            string query = $@"
            SELECT * FROM [User] WHERE Id = '{id}' AND Pwd = '{pwd}';";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.Id = reader.GetString(0);
                        user.Pwd = reader.GetString(1);
                        user.Created = reader.GetString(2).Substring(0, 10);
                        user.Score = reader.GetInt32(3);
                    }

                }

                cnn.Close();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public List<User> getTop5()
        {
            List<User> userslst = new List<User>(); 
            string query = @"SELECT TOP 5
	                        Id,
                            Score
                        FROM
                            [User]
                        ORDER BY 
                            Score DESC;";

            cnn.Open();
            SqlCommand command = new SqlCommand(query, cnn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    userslst.Add(new User
                    {
                        Id = reader.GetString(0),
                        Score = reader.GetInt32(1)
                    });
                }
            }

            cnn.Close();
            return userslst;
        }
        public User createUser(string id, string pwd)
        {
            User user = null;
            string query = $@"
            INSERT INTO [User] (Id, Pwd, Created, Score)
            VALUES ('{id}', '{pwd}', '"  + DateTime.Now.ToShortDateString() + "',0);";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                command.ExecuteNonQuery();
                cnn.Close();

                return getUser(id, pwd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return user;
            }

        }
        public bool updateScore(string id, int score)
        {
            string query = $@"
               UPDATE [User] SET Score = {score} WHERE Id = '{id}'";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                command.ExecuteNonQuery();
                cnn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool deleteUser(string id)
        {
            string query = $@"
                DELETE [User] WHERE Id = '{id}'";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                command.ExecuteNonQuery();
                cnn.Close();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
